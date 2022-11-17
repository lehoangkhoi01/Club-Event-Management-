using Infrastructure.Services.EventServices;
using Infrastructure.Services.FirebaseServices.NotificationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services.BackgroundService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private Timer? _soonHappenTimer = null;
        private Timer? _changeEventToPastTimer = null;
        private readonly IServiceProvider Services;

        public TimedHostedService(IServiceProvider services)
        {
            Services = services;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _soonHappenTimer = new Timer(DoNotifySoonHappen, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            _changeEventToPastTimer = new Timer(DoChangeEventToPast, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private void DoNotifySoonHappen(object? state)
        {
            using (var scope = Services.CreateScope())
            {
                var notificationService =
                    scope.ServiceProvider
                        .GetRequiredService<NotificationService>();
                var db =
                    scope.ServiceProvider
                        .GetRequiredService<ClubEventManagementContext>();

                //get all published event/ eventActivity that is going to happen
                var soonHappenEvents = db.Events.AsNoTracking().Where(ev => 
                                                ev.EventStartTime > DateTime.Now.AddMinutes(-5) 
                                                && ev.EventStartTime < DateTime.Now
                                                && ev.EventStatus.EventStatusName == EventStatusEnum.PUBLISHED.ToString())
                .ToList();
            var soonHappenActivities = db.EventActivities.Include(activity => activity.Event).AsNoTracking().Where(activity => 
                                                activity.StartTime > DateTime.Now.AddMinutes(-5) 
                                                && activity.StartTime < DateTime.Now)
                .ToList();

            //Notify



                soonHappenActivities.ForEach(activity =>
                {
                    var noti = new NotificationDto
                    {
                        ActionType = ActionType.SOON.ToString(),
                        EventId = activity.Event.Id,
                        Eventname = activity.Event.EventName,
                        SubjectId = activity.EventActivityId,
                        SubjectName = activity.EventActivityName,
                        SubjectType = SubjectType.ACTIVITY.ToString(),
                    };
                    notificationService.PublishNotification(noti);
                });

                soonHappenEvents.ForEach(ev =>
                {
                    var noti = new NotificationDto
                    {
                        ActionType = ActionType.SOON.ToString(),
                        EventId = ev.Id,
                        Eventname = ev.EventName,
                        SubjectId = ev.Id,
                        SubjectName = ev.EventName,
                        SubjectType = SubjectType.EVENT.ToString()
                    };
                    notificationService.PublishNotification(noti);
                });

            }



            
        }

        private void DoChangeEventToPast(object? state)
        {
            using (var scope = Services.CreateScope())
            {
                var db =
                    scope.ServiceProvider
                        .GetRequiredService<ClubEventManagementContext>();

                var eventsToChange = db.Events.AsQueryable().Where(ev =>
                                                ev.EventStatus.EventStatusName == EventStatusEnum.PUBLISHED.ToString()
                                                && ev.EventEndTime < DateTime.Now).ToList();
                var pastStatus = db.EventStatuses.AsQueryable().Where(evStat => evStat.EventStatusName == "PAST").First();
                //update event status
                eventsToChange.ForEach(ev => ev.EventStatus = pastStatus);
                db.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {

            _soonHappenTimer?.Change(Timeout.Infinite, 0);
            _changeEventToPastTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _soonHappenTimer?.Dispose();
            _changeEventToPastTimer?.Dispose();
        }
    }
}
