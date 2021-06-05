namespace ZiTechDev.CommonModel.Engines.CustomResult
{
    public class Successed<T> : ApiResult<T>
    {
        public Successed()
        {
            IsSuccessed = true;
        }

        public Successed(T returnedObject)
        {
            IsSuccessed = true;
            ReturnedObject = returnedObject;
        }
    }
}
