using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Cli.Helpers
{
    public static class Statics
    {

        public static bool Verbose = false;
        public static bool Kubectl = false;


        public static void setGlobals(GlobalSettings gs) {

            Verbose = gs.Verbose != null ? (bool)gs.Verbose : false;
            Kubectl = gs.KubeCtl != null ? (bool)gs.KubeCtl : false;

        }



    }
}
