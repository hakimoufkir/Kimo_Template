using Kimo.ZLApp.Infrastructure.Common;
using Kimo.ZLApp.Infrastructure.Common.Database;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common;

public sealed class TestRepository(IApplicationDbContext context) : RepositoryBase<TestEntity, int>(context);
