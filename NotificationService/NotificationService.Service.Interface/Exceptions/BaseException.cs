﻿using System;

namespace Notification.Service.Interface.Exceptions
{
	public class BaseException : Exception
	{

		public int StatusCode { get; set; }

		public BaseException(string message) : base(message) { }
	}
}
