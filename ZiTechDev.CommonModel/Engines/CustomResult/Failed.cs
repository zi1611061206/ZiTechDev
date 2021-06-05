namespace ZiTechDev.CommonModel.Engines.CustomResult
{
    public class Failed<T> : ApiResult<T>
    {
        public Failed(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
    }
}
