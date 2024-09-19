using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

public class CustomWebSocketMiddleware
{
    private readonly RequestDelegate _next;    
    private readonly IConfiguration _iConfiguration;    

    public CustomWebSocketMiddleware(RequestDelegate next, IConfiguration iConfiguration)
    {
        _next = next;      
        _iConfiguration = iConfiguration;
    } 

    public async Task InvokeAsync(HttpContext context, IHiringRequest iHiringRequest)
    {        
        if (context.WebSockets.IsWebSocketRequest)
        {
            using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
            {
                // Handle WebSocket connections here
                await HandleWebSocketConnection(iHiringRequest, webSocket);
            }
        }
        else
        {
            await _next(context);
        }
    }

    private async Task HandleWebSocketConnection(IHiringRequest iHiringRequest, WebSocket webSocket)
    {
        try
        {
            // Implement SQL dependency here to listen for database changes
            // When a change occurs, send the data to the WebSocket

            // Example: Continuously listen for changes in SQL Server
            var connectionString = _iConfiguration["ConnectionStrings:TalentConnectAdminDBConnection"];
            //var connectionString = "server=3.218.6.134;database=TC_QA;user id=QA_reena;password=reena@uplers;MultipleActiveResultSets=True;";
            var sqlDependency = new SqlDependency();
            SqlDependency.Start(connectionString);
            sqlDependency.OnChange += async (sender, e) =>
            {
                // Fetch updated data from the database and send it to the WebSocket
                var data = await FetchDataFromDatabase(iHiringRequest).ConfigureAwait(false);
                if (data != null)
                {                    
                    var dataBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                    var segment = new ArraySegment<byte>(dataBytes);

                    await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            };

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand($"SELECT [HRID],[Total_Talent],[CreatedByDateTime],[ModifiedByDateTime] from [dbo].[gen_ATS_HRStatus_Details] ", connection))
                {
                    // Register the SQL dependency
                    command.Notification = null;                    
                    sqlDependency.AddCommandDependency(command);
                    await command.ExecuteNonQueryAsync();
                }
            }

            // Handle WebSocket messages and disconnections here
            while (webSocket.State == WebSocketState.Open)
            {
                var buffer = new byte[1024];
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                // Handle incoming WebSocket messages if needed
            }
        }
        catch(Exception ex) 
        {
            throw ex;
        }
    }

    private async Task<dynamic> FetchDataFromDatabase(IHiringRequest iHiringRequest)
    {
        var data = await iHiringRequest.Get_HiringRequest_SLADetails(0).ConfigureAwait(false);
        return data;
    }
}
