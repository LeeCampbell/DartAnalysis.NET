﻿using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DanTup.DartAnalysis
{
	// This class is the entry point for consumers. In order to keep it slim, each
	// command file contains its own implementation of an extension method to handle
	// sending request/receiving response and mapping it back to a nice .NET type.

	/// <summary>
	/// Wraps the Google Dart Analysis Service providing a strongly-typed .NET
	/// interface.
	/// </summary>
	public class DartAnalysisService : IDisposable
	{
		/// <summary>
		/// The underlying service for sending requests/responses.
		/// </summary>
		internal AnalysisServiceWrapper Service { get; private set; }

		#region Events

		readonly Subject<ServerStatusNotification> serverStatus = new Subject<ServerStatusNotification>();
		public IObservable<ServerStatusNotification> ServerStatusNotification { get { return serverStatus.AsObservable(); } }

		readonly Subject<AnalysisErrorsNotification> analysisErrors = new Subject<AnalysisErrorsNotification>();
		public IObservable<AnalysisErrorsNotification> AnalysisErrorsNotification { get { return analysisErrors.AsObservable(); } }

		readonly Subject<AnalysisHighlightsNotification> analysisHighlights = new Subject<AnalysisHighlightsNotification>();
		public IObservable<AnalysisHighlightsNotification> AnalysisHighlightsNotification { get { return analysisHighlights.AsObservable(); } }

		readonly Subject<AnalysisNavigationNotification> analysisNavigation = new Subject<AnalysisNavigationNotification>();
		public IObservable<AnalysisNavigationNotification> AnalysisNavigationNotification { get { return analysisNavigation.AsObservable(); } }

		readonly Subject<AnalysisOutlineNotification> analysisOutline = new Subject<AnalysisOutlineNotification>();
		public IObservable<AnalysisOutlineNotification> AnalysisOutlineNotification { get { return analysisOutline.AsObservable(); } }

		#endregion

		/// <summary>
		/// Launches the Google Dart Analysis Service using the provided SDK and script.
		/// </summary>
		/// <param name="sdkFolder">The location of the Dart SDK.</param>
		/// <param name="serverScript">The location of the Dart script that runs the Analysis Service.</param>
		/// <param name="eventHandler">A handler for events raised by the Analysis Service.</param>
		public DartAnalysisService(string sdkFolder, string serverScript)
		{
			this.Service = new AnalysisServiceWrapper(sdkFolder, serverScript, HandleEvent);
		}

		void HandleEvent(Event notification)
		{
			if (notification is Event<ServerStatusEvent>)
				TryRaiseEvent(serverStatus, () => ((Event<ServerStatusEvent>)notification).@params.AsNotification());
			else if (notification is Event<AnalysisErrorsEvent>)
				TryRaiseEvent(analysisErrors, () => ((Event<AnalysisErrorsEvent>)notification).@params.AsNotification());
			else if (notification is Event<AnalysisHighlightsEvent>)
				TryRaiseEvent(analysisHighlights, () => ((Event<AnalysisHighlightsEvent>)notification).@params.AsNotification());
			else if (notification is Event<AnalysisNavigationEvent>)
				TryRaiseEvent(analysisNavigation, () => ((Event<AnalysisNavigationEvent>)notification).@params.AsNotification());
			else if (notification is Event<AnalysisOutlineEvent>)
				TryRaiseEvent(analysisOutline, () => ((Event<AnalysisOutlineEvent>)notification).@params.AsNotification());
		}

		void TryRaiseEvent<T>(Subject<T> subject, Func<T> createNotification)
		{
			try
			{
				subject.OnNext(createNotification());
			}
			catch (Exception ex)
			{
				subject.OnError(ex);
			}
		}

		#region OMG DO WE STILL HAVE TO DO THIS?

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Service.Dispose();
			}
		}

		#endregion
	}
}
