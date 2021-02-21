using System;

namespace OpenEvent.Web.Models.Analytic
{
    public abstract class AnalyticEvent
    {
        public abstract Guid Id { get; set; }
        public abstract DateTime Created { get; set; }
    }
}