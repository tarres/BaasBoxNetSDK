using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaasBoxNet
{
    public interface IBaasBoxPlugins
    {
        Task<string> Get(string pluginName, string parameters);
    }
}
