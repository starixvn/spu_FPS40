namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Implement
{
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
    using Fanex.NAP.FPS.Renovation.Model;

    public class DataTransformService : IDataTransformService
    {
        private readonly DataAccess.StagingRawTransactionDataAccess stagingRawTransaction;
  
        public DataTransformService()
        {
            stagingRawTransaction = new DataAccess.StagingRawTransactionDataAccess();
        }

        public StagingRawTransactionModel GetStagingRawTransaction(int isProcessed, int numberOfTicket)
        {
            var result = stagingRawTransaction.GetStagingRawTrans(isProcessed, numberOfTicket);

            System.Console.WriteLine("1.Get Raw Transaction");

            return result;
        }
    }
}
