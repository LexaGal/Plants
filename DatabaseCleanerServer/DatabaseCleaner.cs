using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Quartz;

namespace DatabaseCleanerServer
{
    [DisallowConcurrentExecution]
    public class DatabaseCleaner : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //    using (
            //        MeasuringMessageMappingRepository measuringMessageMappingRepository =
            //            new MeasuringMessageMappingRepository())
            //    {
            //        measuringMessageMappingRepository.DeleteMany();
            //    }
            using (
                var sc =
                    new SqlConnection(
                        ConfigurationManager.ConnectionStrings["PlantingDb"].ConnectionString))
            {
                if (sc.State == ConnectionState.Closed)
                {
                    sc.Open();
                }
                //using (var tr = sc.BeginTransaction(IsolationLevel.Snapshot))
                //{
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("RemoveMessages", sc))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@param", SqlDbType.Int).Value = 1000;
                            //cmd.Transaction = tr;
                            cmd.ExecuteNonQuery();
                        }
                        //tr.Commit();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        //tr.Rollback();
                    }
                //}
            }
        }
    }
}
