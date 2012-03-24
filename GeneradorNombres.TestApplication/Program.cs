﻿﻿/**
  * GeneradorNombres
  * Aplicación de Prueba
  * Program.cs
  *
  * Copyright 2012 Javier Campos
  * http://www.javiercampos.info
  * 
  * Licensed under the Apache License, Version 2.0 (the "License");
  * you may not use this file except in compliance with the License.
  * 
  * You may obtain a copy of the License at
  * 
  * http://www.apache.org/licenses/LICENSE-2.0
  * 
  * Unless required by applicable law or agreed to in writing, software
  * distributed under the License is distributed on an "AS IS" BASIS,
  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  * See the License for the specific language governing permissions and
  * limitations under the License.    
  *    
  **/

using System;

namespace Jcl.Util.GeneradorNombres.TestApplication
{
  /// <summary>
  /// Pequeña aplicación para probar la librería
  /// </summary>
  class Program
  {
    static void Main()
    {
      EscribeTitulo("Nombres de varón con dos apellidos", false);
      for (var i = 0; i < 10; i++)
        Console.Write("{1}{0}", GeneradorNombresCastellano.Generar(100, 100), i != 0 ? ", " : "");

      EscribeTitulo("Nombres de varón con un apellido");
      for (var i = 0; i < 10; i++)
        Console.Write("{1}{0}", GeneradorNombresCastellano.Generar(100, 0), i != 0 ? ", " : "");

      EscribeTitulo("Nombres de mujer con dos apellidos");
      for (var i = 0; i < 10; i++)
        Console.Write("{1}{0}", GeneradorNombresCastellano.Generar(0, 100), i != 0 ? ", " : "");

      EscribeTitulo("Nombres de mujer con un apellido");
      for (var i = 0; i < 10; i++)
        Console.Write("{1}{0}", GeneradorNombresCastellano.Generar(0, 0), i != 0 ? ", " : "");

      EscribeTitulo("Nombres aleatorios");
      for (var i = 0; i < 30; i++)
        Console.Write("{1}{0}", GeneradorNombresCastellano.Generar(50, 50), i != 0 ? ", " : "");

      Console.WriteLine();
    }

    private static void EscribeTitulo(string titulo, bool espacioBlancoEncima = true)
    {
      var formato = "{0}{2}{1}";
      if (espacioBlancoEncima)
      {
        formato = "{2}{2}" + formato;
      }
      Console.WriteLine(formato, titulo, "".PadLeft(titulo.Length, '='), Environment.NewLine);
    }

  }
}
