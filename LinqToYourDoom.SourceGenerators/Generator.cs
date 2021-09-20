using System;
using System.Text;
using LinqToYourDoom.SourceGenerators.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace LinqToYourDoom.SourceGenerators {
	[Generator]
	public sealed class Generator : ISourceGenerator {
		public void Initialize(GeneratorInitializationContext context) {}

		public void Execute(GeneratorExecutionContext context) {
			GenerateCsFile(context, nameof(BooleanExtensions), BooleanExtensions.Generate);
			GenerateCsFile(context, nameof(EnumerableExtensions), EnumerableExtensions.Generate);
			GenerateCsFile(context, nameof(IRandomD), IRandomD.Generate);
			GenerateCsFile(context, nameof(MathD), MathD.Generate);
			GenerateCsFile(context, nameof(NumberExtensions), NumberExtensions.Generate);
			GenerateCsFile(context, nameof(ObjectExtensions), ObjectExtensions.Generate);
			GenerateCsFile(context, nameof(RandomD), RandomD.Generate);
			GenerateCsFile(context, nameof(TryFunc), TryFunc.Generate);
			GenerateCsFile(context, nameof(ValueTupleExtensions), ValueTupleExtensions.Generate);
		}

		static void GenerateCsFile(GeneratorExecutionContext context, string className, Action<StringBuilder> writer) {
			var code = new StringBuilder();

			writer.Invoke(code);

			context.AddSource(className + ".g.cs", SourceText.From(code.ToString(), Encoding.UTF8));
		}
	}
}
