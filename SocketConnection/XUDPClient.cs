using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommonFrontEnd.SocketConnection
{
    public class XUDPClient : UdpClient
    {
        /// <summary>
        /// This method creates a UDP Client which connects to the specified Remote Server and Port.
        /// </summary>
        /// <param name="pServerAddress">The Remote Server</param>
        /// <param name="pSocket">The Remote Port</param>
        /// <param name="pTimeOut">The Time out during receive in milliseconds.</param>
        public XUDPClient(String pServerAddress, int pSocket, int pTimeOut) : base(pServerAddress, pSocket)
        {
            this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, pTimeOut);
        }
        /// <summary>
        /// This method creates a UDP Client which connects to the specified Remote Server and Port.
        /// </summary>
        /// <param name="pServerAddress">The Remote Server</param>
        /// <param name="pSocket">The Remote Port</param>
        public XUDPClient(String pServerAddress, int pSocket) : base(pServerAddress, pSocket)
        {
        }
        /// <summary>
        /// This method creates a UDP Client which binds to the specified Local Port.
        /// </summary>
        /// <param name="pServerAddress">The Remote Server</param>
        public XUDPClient(int pLocalPort) : base(pLocalPort)
        {
        }
        /// <summary>
        /// This method creates a UDP Client which binds to the specified Local Port.
        /// </summary>
        /// <param name="pServerAddress">The Remote Server</param>
        public XUDPClient(int pLocalPort, int pTimeOut) : base(pLocalPort)
        {
            this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, pTimeOut);
        }
        public XUDPClient(IPEndPoint pIPEndPoint) : base(pIPEndPoint)
        {
        }
        public XUDPClient() : base()
        {
        }
        /// <summary>
        /// This method sets the Receive Timeout period for the UDPClient.
        /// </summary>
        /// <param name="pTimeOutInterval">The Timeout Period in Milliseconds.</param>
        public void setReceiveTimeOut(int pTimeOutInterval)
        {
            this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, pTimeOutInterval);
        }
        /// <summary>
        /// This method gets the Local End Point used by the UDP Client for communication.
        /// </summary>
        /// <returns>Returns the Local Port</returns>
        public int getLocalPort()
        {
            IPEndPoint lIPEndPoint = (IPEndPoint)base.Client.LocalEndPoint;
            return lIPEndPoint.Port;
        }
    }
}
