﻿namespace DanTup.DartAnalysis
{
	/// <summary>
	/// Represents a response from the Analysis Service.
	/// </summary>
	class Response
	{
		public string id = "0";
	}

	/// <summary>
	/// Represents a response from the Analysis Service that has a typed result.
	/// </summary>
	/// <typeparam name="T">The type of the result included in the response.</typeparam>
	class ResponseWithResult<T> : Response
	{
		public T result;
	}

	/// <summary>
	/// Represents a valid error response from the Analsysis Service.
	/// </summary>
	class ErrorResponse
	{
		public string id = "0";
		public ErrorResult error;
	}

	/// <summary>
	/// Represents an error code/message in an error response from the Analysis Service.
	/// </summary>
	class ErrorResult
	{
		public int code;
		public string message;
	}
}