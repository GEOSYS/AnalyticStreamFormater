using FluentValidation;

namespace Geosys.NotificationDataExporter.WebHook.Domain.Models.Validators
{
	public class AnalyticsListValidator : AbstractValidator<List<AnalyticsDto>>
	{
		public AnalyticsListValidator()
		{
			RuleForEach(x => x).SetValidator(new AnalyticsValidator());
		}
	}

	public class AnalyticsValidator : AbstractValidator<AnalyticsDto>
	{
		public AnalyticsValidator()
		{
			RuleFor(x => x.Entity).NotNull();
            RuleFor(x => x.Entity).SetValidator(new EntityValidator());
			RuleFor(x => x.Timestamp).NotEqual(DateTime.MinValue);
			RuleFor(x => x.Schema).NotNull();
			RuleFor(x => x.Schema).SetValidator(new AnalyticsSchemaValidator());
		}
	}

    public class EntityValidator : AbstractValidator<EntityDto>
    {
        public EntityValidator()
        {
			RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Type).NotNull();
		}
    }

	public class AnalyticsSchemaValidator : AbstractValidator<AnalyticsSchemaDto>
	{
		public AnalyticsSchemaValidator()
		{
			RuleFor(x => x.Id).NotNull();
			RuleFor(x => x.Version).NotNull();
            RuleFor(x => x.Version).SetValidator(new VersionValidator());
		}
	}
    public class VersionValidator : AbstractValidator<SchemaVersionDto>
    {
        public VersionValidator()
        {
            RuleFor(x => x.Minor).NotNull();
            RuleFor(x => x.Major).NotNull();
        }
    }
}
