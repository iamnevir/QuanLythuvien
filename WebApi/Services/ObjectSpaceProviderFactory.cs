using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;

namespace WebApi.WebApi.Core;

public sealed class ObjectSpaceProviderFactory : IObjectSpaceProviderFactory {
    readonly ITypesInfo typesInfo;
    readonly IXpoDataStoreProvider dataStoreProvider;

    public ObjectSpaceProviderFactory(ITypesInfo typesInfo, IXpoDataStoreProvider dataStoreProvider) {
        this.typesInfo = typesInfo;
        this.dataStoreProvider = dataStoreProvider;
    }

    public IEnumerable<IObjectSpaceProvider> CreateObjectSpaceProviders() {
        yield return new XPObjectSpaceProvider(dataStoreProvider, true);
        yield return new NonPersistentObjectSpaceProvider(typesInfo, null);
    }
}
