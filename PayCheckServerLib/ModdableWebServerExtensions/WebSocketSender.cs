using ModdableWebServer.Helper;
using ModdableWebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PayCheckServerLib.ModdableWebServerExtensions
{
	public static class WebSocketSender
	{
		public static void SendWebSocketText(this WebSocketStruct socketStruct, string text)
		{
			if (socketStruct.IsConnected)
			{
				switch (socketStruct.Enum)
				{
					case WSEnum.WS:
						socketStruct.WS_Session?.GetType().GetRuntimeMethod("SendText", new Type[1] { typeof(string) })?.Invoke(socketStruct.WS_Session, new object[1] { text });
						DebugPrinter.Debug("[SendWebSocketText] WS Text Sent!");
						break;
					case WSEnum.WSS:
						socketStruct.WSS_Session?.GetType().GetRuntimeMethod("SendText", new Type[1] { typeof(string) })?.Invoke(socketStruct.WSS_Session, new object[1] { text });
						DebugPrinter.Debug("[SendWebSocketText] WSS Text Sent!");
						break;
				}
			}
		}
		public static void SendWebSocketByteArray(this WebSocketStruct socketStruct, byte[] bytes)
		{
			if (socketStruct.IsConnected)
			{
				switch (socketStruct.Enum)
				{
					case WSEnum.WS:
						socketStruct.WS_Session?.GetType().GetRuntimeMethod("SendBinary", new Type[1] { typeof(byte[]) })?.Invoke(socketStruct.WS_Session, new object[1] { bytes });
						DebugPrinter.Debug("[SendWebSocketByteArray] WS Binary Sent!");
						break;
					case WSEnum.WSS:
						socketStruct.WSS_Session?.GetType().GetRuntimeMethod("SendBinary", new Type[1] { typeof(byte[]) })?.Invoke(socketStruct.WSS_Session, new object[1] { bytes });
						DebugPrinter.Debug("[SendWebSocketByteArray] WSS Binary Sent!");
						break;
				}
			}
		}
	}
}
