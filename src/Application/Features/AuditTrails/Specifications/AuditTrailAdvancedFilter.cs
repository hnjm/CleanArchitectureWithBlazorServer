﻿using CleanArchitecture.Blazor.Application.Common.Security;

namespace CleanArchitecture.Blazor.Application.Features.AuditTrails.Specifications;

public enum AuditTrailListView
{
    [Description("الكل")] [Display(Name="الكل")] All,
    [Description("My Change Histories")] My,
    [Description("Created Today")] TODAY,
    [Description("View of the last 30 days")] LAST_30_DAYS
}

public class AuditTrailAdvancedFilter : PaginationFilter
{
    public AuditType? AuditType { get; set; }
    public AuditTrailListView ListView { get; set; } = AuditTrailListView.All;
    public UserProfile? CurrentUser { get; set; }
}