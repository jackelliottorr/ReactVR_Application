using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactVR_API.Common.Models;

namespace Assets._PROJECT.Scripts.HelperClasses
{
    public static class SessionData
    {
        public static LevelConfigurationViewModel LevelConfigurationViewModel { get; set; }

        public static bool DemoMode = false;
    }
}
