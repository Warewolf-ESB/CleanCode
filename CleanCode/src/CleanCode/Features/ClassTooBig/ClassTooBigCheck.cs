using CleanCode.Resources;
using CleanCode.Settings;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.CSharp.Stages;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace CleanCode.Features.ClassTooBig
{
    public class ClassTooBigCheck : SimpleCheck<IClassDeclaration, int>
    {
        public ClassTooBigCheck(IContextBoundSettingsStore settingsStore)
            : base(settingsStore)
        {
        }

        protected override void ExecuteCore(IClassDeclaration classDeclaration, IHighlightingConsumer consumer)
        {
            var maxLength = Threshold;

            var statementCount = classDeclaration.CountChildren<IMethodDeclaration>();
            if (statementCount > maxLength)
            {
                var declarationIdentifier = classDeclaration.NameIdentifier;
                var documentRange = declarationIdentifier.GetDocumentRange();
                var highlighting = new Highlighting(Warnings.ClassTooBig);
                consumer.AddHighlighting(highlighting, documentRange);
            }
        }

        protected override int Threshold
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ClassTooBigMaximum); }
        }

        protected override bool IsEnabled
        {
            get { return SettingsStore.GetValue((CleanCodeSettings s) => s.ClassTooBigEnabled); }
        }
    }
}