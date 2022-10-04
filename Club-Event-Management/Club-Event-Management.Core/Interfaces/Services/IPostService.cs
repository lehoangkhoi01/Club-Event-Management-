using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Services
{
    public interface IPostService
    {
        public Task Add(EventPost post);
        public Task<IEnumerable<EventPost>> GetAllPost();
    }
}
