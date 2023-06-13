using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Security;
using Microsoft.EntityFrameworkCore;

namespace DXApplication1.WebApi.Core;

public sealed class ObjectSpaceProviderFactory : IObjectSpaceProviderFactory {
    readonly ITypesInfo typesInfo;
    readonly IXafDbContextFactory<QuanLyBanHang.Module.BusinessObjects.DXApplication1EFCoreDbContext> dbFactory;

    public ObjectSpaceProviderFactory(ITypesInfo typesInfo, IXafDbContextFactory<QuanLyBanHang.Module.BusinessObjects.DXApplication1EFCoreDbContext> dbFactory) {
        this.typesInfo = typesInfo;
        this.dbFactory = dbFactory;
    }

    IEnumerable<IObjectSpaceProvider> IObjectSpaceProviderFactory.CreateObjectSpaceProviders() {
        yield return new EFCoreObjectSpaceProvider<QuanLyBanHang.Module.BusinessObjects.DXApplication1EFCoreDbContext>(dbFactory, typesInfo);
        yield return new NonPersistentObjectSpaceProvider(typesInfo, null);
    }
}
