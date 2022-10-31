using System;
using System.Collections.Generic;
using System.Text;

namespace k2s.Models
{
    public class BaseResult<T>
    {
        public ActionResultType Result;
        public string Msg;
        public T Content = default;



        public bool isSuccess() => Result == ActionResultType.Success;
        public bool HasContent() => Content != null;


        public bool isOk() => new List<ActionResultType>() { ActionResultType.Success, ActionResultType.Warning }.Contains(Result);

        public static BaseResult<T> NewSuccess(T content, string msg = "") => new BaseResult<T>() { Result = ActionResultType.Success, Msg = msg, Content = content != null ? content : default };
        public static BaseResult<T> NewWarning(T content, string msg = "") => new BaseResult<T>() { Result = ActionResultType.Warning, Msg = msg, Content = content != null ? content : default };
        public static BaseResult<T> NewError(T content, string msg = "") => new BaseResult<T>() { Result = ActionResultType.Error, Msg = msg, Content = content != null ? content : default };
        public static BaseResult<T> NewFatal(T content, string msg = "") => new BaseResult<T>() { Result = ActionResultType.Fatal, Msg = msg, Content = content != null ? content : default };


    }

    public class BaseResult
    {
        public ActionResultType Result;
        public string Msg;
        



        public bool isSuccess() => Result == ActionResultType.Success;
        public bool HasContent() => false;


        public bool isOk() => new List<ActionResultType>() { ActionResultType.Success, ActionResultType.Warning }.Contains(Result);

        public static BaseResult NewSuccess(string msg = "") => new BaseResult() { Result = ActionResultType.Success, Msg = msg};
        public static BaseResult NewWarning(string msg = "") => new BaseResult() { Result = ActionResultType.Warning, Msg = msg };
        public static BaseResult NewError(string msg = "") => new BaseResult() { Result = ActionResultType.Error, Msg = msg };
        public static BaseResult NewFatal( string msg = "") => new BaseResult() { Result = ActionResultType.Fatal, Msg = msg };


    }

    public enum ActionResultType
    {

        Success,
        Warning,
        Error,
        Fatal

    }
}
