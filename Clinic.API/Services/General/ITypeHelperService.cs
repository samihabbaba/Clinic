namespace Clinic.Services.General
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}