﻿﻿/**
  * GeneradorNombres
  * Pruebas unitarias
  * GeneradorNombresCastellanoTests.cs
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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jcl.Util.GeneradorNombres.UnitTests
{
  [TestClass]
  public class GeneradorNombresCastellanoTests
  {
    [TestMethod]
    public void DeberiaDevolverNombreVaron()
    {
      var nombre = GeneradorNombresCastellano.GenerarNombre(100);
      Assert.IsTrue(GeneradorNombresCastellano.CatalogoPalabras.NombresVaron.Contains(nombre));
    }

    [TestMethod]
    public void DeberiaDevolverNombreHembra()
    {
      var nombre = GeneradorNombresCastellano.GenerarNombre(0);
      Assert.IsTrue(GeneradorNombresCastellano.CatalogoPalabras.NombresHembra.Contains(nombre));
    }

    [TestMethod]
    public void DeberiaDevolverUnApellido()
    {
      var apellido = GeneradorNombresCastellano.GenerarApellido(0);
      Assert.IsTrue(apellido.Split(' ').Count() == 1);
    }

    [TestMethod]
    public void DeberiaDevolverDosApellidos()
    {
      var apellido = GeneradorNombresCastellano.GenerarApellido(100);
      Assert.IsTrue(apellido.Split(' ').Count() == 2);
    }

    [TestMethod]
    public void DeberiaDevolverUnApellidoCompuesto()
    {
      var apellido = GeneradorNombresCastellano.GenerarApellido(0, 100);
      Assert.IsTrue(apellido.Split('-').Count() == 2);
    }

    [TestMethod]
    public void DeberiaDevolverDosApellidosCompuestos()
    {
      var apellido = GeneradorNombresCastellano.GenerarApellido(100, 100);
      Assert.IsTrue(apellido.Split('-').Count() == 3);
    }
  }
}
