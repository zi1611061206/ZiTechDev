namespace ZiTechDev.CommonModel.Engines.CustomResult
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
        public T ReturnedObject { get; set; }
    }
}
