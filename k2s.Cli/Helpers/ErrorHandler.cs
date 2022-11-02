using k2s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Cli.Helpers
{
    public static class ErrorHandler
    {

        public static void HandleResult(BaseResult res,string prefix="", bool verbose=false) {

            if (!verbose) { verbose = Statics.Verbose; }

            if (res.isSuccess()) { 
                
                if(verbose) Outputs.Success(string.IsNullOrWhiteSpace(prefix)? "Success":prefix, res.Msg);
                return;
            } else {

                if (res.isOk())
                {

                    if (verbose) Outputs.Warning(string.IsNullOrWhiteSpace(prefix) ? "Warning" : prefix, res.Msg);
                    return;

                }
                else {




                    if (res.Result == ActionResultType.Fatal)
                    {
                        Outputs.Error(string.IsNullOrWhiteSpace(prefix) ? "Fatal" : prefix, res.Msg);
                        Environment.Exit(666);
                        return;
                    }
                    else {
                        Outputs.Error(string.IsNullOrWhiteSpace(prefix) ? "Error" : prefix, res.Msg);
                        Environment.Exit(1);

                        return;
                    }

                }





            }



        }



    }

    public static class ErrorHandler<T>
    {

        public static void HandleResult(BaseResult<T> res, string prefix = "", bool verbose = false)
        {
            if (!verbose) { verbose = Statics.Verbose; }
            if (res.isSuccess())
            {

                if (verbose) Outputs.Success(string.IsNullOrWhiteSpace(prefix) ? "Success" : prefix, res.Msg);
                return;
            }
            else
            {

                if (res.isOk())
                {

                    if (verbose) Outputs.Warning(string.IsNullOrWhiteSpace(prefix) ? "Warning" : prefix, res.Msg);
                    return;

                }
                else
                {




                    if (res.Result == ActionResultType.Fatal)
                    {
                        Outputs.Error(string.IsNullOrWhiteSpace(prefix) ? "Fatal" : prefix, res.Msg);
                        Environment.Exit(1);
                        return;
                    }
                    else
                    {
                        Outputs.Error(string.IsNullOrWhiteSpace(prefix) ? "Error" : prefix, res.Msg);
                        return;
                    }

                }





            }



        }



    }

}
