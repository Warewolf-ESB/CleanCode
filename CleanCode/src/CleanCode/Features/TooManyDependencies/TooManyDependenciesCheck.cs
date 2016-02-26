using System.Linq;
using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;

namespace CleanCode.Features.TooManyDependencies
{
    public class TooManyDependenciesCheck : SimpleCheck<IConstructorDeclaration, int>
    {
        public TooManyDependenciesCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IConstructorDeclaration constructorDeclaration, IHighlightingConsumer consumer)
        {
            var maxDependencies = Threshold;

            var depedencies = constructorDeclaration.ParameterDeclarations.Select(
                declaration => declaration.DeclaredElement != null &&
                               declaration.DeclaredElement.Type.IsInterfaceType());

            var dependenciesCount = depedencies.Count();

            if (dependenciesCount > maxDependencies)
            {
                var highlighting = new Highlighting(Warnings.TooManyDependencies);
                consumer.AddHighlighting(highlighting, constructorDeclaration.GetNameDocumentRange());
            }
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDependenciesMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.TooManyDependenciesMaximumEnabled); }
        }
    }
}