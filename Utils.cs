using System;
using DSL;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGenerator
{
    public static class Utils
    {
        public static SyntaxKind ModifierToSyntaxKind(Modifier modifier)
        {
            SyntaxKind result;

            switch (modifier)
            {
                case Modifier.Public:
                {
                    result = SyntaxKind.PublicKeyword;
                    break;
                }
                case Modifier.Abstract:
                {
                    result = SyntaxKind.AbstractKeyword;
                    break;
                }
                case Modifier.Final:
                {
                    result = SyntaxKind.SealedKeyword;
                    break;
                }
                case Modifier.Protected:
                {
                    result = SyntaxKind.ProtectedKeyword;
                    break;
                }
                case Modifier.Static:
                {
                    result = SyntaxKind.StaticKeyword;
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}
