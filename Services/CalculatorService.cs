using System.ServiceModel;

namespace LibraryAPI.Services
{
  [ServiceContract]
  public interface ICalculator
  {
      [OperationContract]
      string Hello(string name);
  }
}