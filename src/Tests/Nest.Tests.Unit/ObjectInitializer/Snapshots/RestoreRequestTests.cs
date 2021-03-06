﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Elasticsearch.Net;
using FluentAssertions;
using NUnit.Framework;

namespace Nest.Tests.Unit.ObjectInitializer.Snapshots
{
	[TestFixture]
	public class RestoreRequestTests : BaseJsonTests
	{
		private readonly IElasticsearchResponse _status;

		public RestoreRequestTests()
		{
			var request = new RestoreRequest("repos", "snap")
			{
				IgnoreUnavailable = true,
				IncludeGlobalState = true,
				WaitForCompletion = true,
				RenamePattern = "index_(.+)",
				RenameReplacement = "index_restored_$1"
			};
			var response = this._client.Restore(request);
			this._status = response.ConnectionStatus;
		}

		[Test]
		public void Url()
		{
			this._status.RequestUrl.Should()
				.EndWith("/_snapshot/repos/snap/_restore?wait_for_completion=true");
			this._status.RequestMethod.Should().Be("POST");
		}
		
		[Test]
		public void RestoreBody()
		{
			this.JsonEquals(this._status.Request, MethodBase.GetCurrentMethod());
		}
	}
}
