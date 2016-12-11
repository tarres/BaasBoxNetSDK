using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaasBoxNet
{
    internal class BaasBoxPlugins : IBaasBoxPlugins
    {
        private readonly BaasBox _box;

        public BaasBoxPlugins(BaasBox box)
        {
            _box = box;
        }

        public async Task<string> Get(string pluginName, string parameters)
        {
            var requestUrl = string.Format("plugin/{0}?{1}", pluginName, parameters);
            return await _box.RestService.GetAsync<string>(requestUrl, CancellationToken.None);
        }
    }
}