﻿﻿/**
  * GeneradorNombres
  * GeneradorNombresCastellano.cs
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
using System.Globalization;

namespace Jcl.Util.GeneradorNombres
{
  /// <summary>
  /// Generador de nombres en Castellano
  /// </summary>
  public static class GeneradorNombresCastellano
  {
    /// <summary>
    /// Conversión a "Title Case" de español si está disponible. Caso contrario usa la cultura del sistema.
    /// </summary>
    /// <param name="value">Cadena a convertir.</param>
    /// <returns>Cadena convertida a TitleCase</returns>
    private static string Capitalize(string value)
    {
      CultureInfo ci;
      try
      {
        ci = new CultureInfo("es-ES", false);
      }
      catch (CultureNotFoundException)
      {
        ci = CultureInfo.CurrentCulture;
      }
      return ci.TextInfo.ToTitleCase(value);
    }

    /// <summary>
    /// Random() usa los ticks para la semilla inicial. Si hacemos nuevos objetos Random en menos tiempo de la precisión 
    /// de un tick, obtendremos el mismo seed durante ese tiempo. Podríamos guardar el objeto Random como static, es más 
    /// eficiente para la memoria guardar sólo el próximo seed.
    /// </summary>
    private static int _proximaSemilla;

    /// <summary>
    /// Crea un nuevo objeto Random e inicializa la próxima semilla
    /// </summary>
    /// <returns>Nuevo objeto Random</returns>
    private static Random NuevoRandom()
    {
      var rand = new Random(_proximaSemilla == 0 ? Environment.TickCount : _proximaSemilla);
      _proximaSemilla = rand.Next();
      return rand;
    }

    /// <summary>
    /// Probabilidad entre 100
    /// </summary>
    /// <param name="probabilidad">Probabilidad.</param>
    /// <returns>"probabilidad" entre 100 de devolver 1, o 0 en caso contrario.</returns>
    private static int ProbabilidadEntreCien(int probabilidad)
    {
      return NuevoRandom().Next(100) < probabilidad ? 1 : 0;
    }


    /// <summary>
    /// Generar un nombre aleatorio
    /// </summary>
    /// <param name="probabilidadVaron">Probabilidad (0-100) de que el nombre sea de varón. Por defecto 50.</param>
    /// <returns>El nombre generado.</returns>
    public static string GenerarNombre(int probabilidadVaron = 50)
    {
      var rand = NuevoRandom();
      var coleccionNombres = rand.Next(100) < probabilidadVaron ? CatalogoPalabras.NombresVaron : CatalogoPalabras.NombresHembra;
      return coleccionNombres[rand.Next(coleccionNombres.Length)];
    }

    /// <summary>
    /// Genera un apellido aleatorio.
    /// </summary>
    /// <param name="probabilidadDosApellidos">Probabilidad (0-100) de generar dos apellidos en lugar de sólo uno. Por defecto 0.</param>
    /// <param name="probabilidadApellidoCompuesto">Probabilidad (0-100) de que el/los apellido(s) sea(n) compuesto(s). Por defecto 10.</param>
    /// <param name="incluyeApellidosConEspacios">Incluye apellidos con espacios (p.ej., "De La Fuente", "Da Silva")</param>
    /// <returns>El/los apellido(s) generado(s)</returns>
    public static string GenerarApellido(int probabilidadDosApellidos = 0, int probabilidadApellidoCompuesto = 10, bool incluyeApellidosConEspacios = false)
    {
      var rand = NuevoRandom();

      var resultado = "";
      var numApellidos = 1 + ProbabilidadEntreCien(probabilidadDosApellidos);
      for (var i = 0; i < numApellidos; i++)
      {
        var apellido = "";
        // Creamos un apellido compuesto si lo admitimos
        var numCompuestos = 1 + ProbabilidadEntreCien(probabilidadApellidoCompuesto);
        for (var j = 0; j < numCompuestos; j++)
        {
          string selApellido;
          // Buscamos apellidos hasta que tengamos uno sin espacios si lo hemos seleccionado
          do
          {
            selApellido = CatalogoPalabras.Apellidos[rand.Next(CatalogoPalabras.Apellidos.Length)];
          } while (selApellido.Contains(" ") && !incluyeApellidosConEspacios);
          apellido += (j != 0 ? "-" : "") + selApellido;
        }
        resultado += (i != 0 ? " " : "") + apellido;
      }
      return resultado;

    }

    /// <summary>
    /// Generar un nombre aleatorio.
    /// </summary>
    /// <param name="probabilidadVaron">Probabilidad (0-100) de que el nombre sea varón. Por defecto 0</param>
    /// <param name="probabilidadDosApellidos">Probabilidad (0-100) de generar dos apellidos en lugar de sólo uno. Por defecto 100.</param>
    /// <param name="probabilidadApellidoCompuesto">Probabilidad (0-100) de que el/los apellido(s) sea(n) compuesto(s). Por defecto 10.</param>
    /// <param name="incluyeApellidosConEspacios">Incluye apellidos con espacios (p.ej., "De La Fuente", "Da Silva")</param>
    /// <param name="titleCase">Si es <c>true</c>, devuelve los nombres en Title Case.</param>
    /// <returns>Nombre generado.</returns>
    public static string Generar(int probabilidadVaron = 50, int probabilidadDosApellidos = 100, int probabilidadApellidoCompuesto = 10, bool incluyeApellidosConEspacios = true, bool titleCase = true)
    {
      var rand = new Random(_proximaSemilla == 0 ? Environment.TickCount : _proximaSemilla);
      _proximaSemilla = rand.Next();

      var resultado = GenerarNombre(probabilidadVaron);
      resultado += " " + GenerarApellido(probabilidadDosApellidos, probabilidadApellidoCompuesto, incluyeApellidosConEspacios);
      return titleCase ? Capitalize(resultado) : resultado;
    }

    /// <summary>
    /// Datos extraidos del INE para el año 2010. Nombres y apellidos más comunes en España.
    /// http://www.ine.es/daco/daco42/nombyapel/nombyapel.htm
    /// </summary>
    public static class CatalogoPalabras
    {
      /// <summary>
      /// Nombres de varón
      /// </summary>
      public static readonly string[] NombresVaron = new[]
                                                     {
                                                       "Antonio", "Jose", "Manuel", "Francisco", "Juan", "David",
                                                       "Jose Antonio", "Jose Luis", "Jesus", "Javier",
                                                       "Francisco Javier", "Carlos", "Daniel", "Miguel", "Rafael",
                                                       "Pedro", "Jose Manuel", "Angel", "Alejandro", "Miguel Angel",
                                                       "Jose Maria", "Fernando", "Luis", "Sergio", "Pablo", "Jorge",
                                                       "Alberto", "Juan Carlos", "Juan Jose", "Ramon", "Enrique",
                                                       "Vicente", "Juan Antonio", "Diego", "Raul", "Alvaro", "Joaquin",
                                                       "Adrian", "Andres", "Ivan", "Oscar", "Ruben", "Santiago",
                                                       "Juan Manuel", "Eduardo", "Victor", "Roberto", "Jaime",
                                                       "Francisco Jose", "Alfonso", "Ignacio", "Salvador", "Ricardo",
                                                       "Emilio", "Jordi", "Mario", "Julian", "Julio", "Marcos", "Tomas",
                                                       "Agustin", "Guillermo", "Gabriel", "Jose Miguel", "Felix",
                                                       "Jose Ramon", "Mohamed", "Joan", "Gonzalo", "Marc", "Mariano",
                                                       "Domingo", "Josep", "Ismael", "Cristian", "Juan Francisco",
                                                       "Alfredo", "Sebastian", "Felipe", "Nicolas", "Jose Carlos",
                                                       "Samuel", "Cesar", "Martin", "Jose Angel", "Gregorio",
                                                       "Jose Ignacio", "Aitor", "Victor Manuel", "Hugo", "Luis Miguel",
                                                       "Hector", "Jose Francisco", "Lorenzo", "Juan Luis", "Cristobal",
                                                       "Esteban", "Albert", "Xavier", "Eugenio", "Antonio Jose",
                                                       "Arturo", "Rodrigo", "Iker", "Borja", "Alex", "Valentin",
                                                       "Jose Javier", "Jesus Maria", "Juan Miguel", "Jaume", "German",
                                                       "Antonio Jesus", "Francisco Manuel", "Jonathan", "Adolfo",
                                                       "Pedro Jose", "Jose Vicente", "Benito", "Lucas", "Isaac",
                                                       "Isidro", "Mohammed", "Moises", "Juan Ramon", "Pau", "Juan Pedro"
                                                       , "Bernardo", "Abel", "Ahmed", "Ernesto", "Gerardo", "Pascual",
                                                       "Christian", "Carmelo", "Manuel Jesus", "Sergi", "Mikel",
                                                       "Federico", "Iñigo", "Aaron", "Marcelino", "Bartolome", "Miquel",
                                                       "Antonio Manuel", "Asier", "Francesc", "Israel", "Joel", "Fermin"
                                                       , "Eloy", "Jose Alberto", "Jesus Manuel", "Aurelio",
                                                       "Luis Alberto", "Jon", "Eric", "Benjamin", "Juan Jesus", "Pere",
                                                       "Jonatan", "Gerard", "Mateo", "Omar", "Eusebio", "Lluis", "Oriol"
                                                       , "Josep Maria", "Antoni", "Jacinto", "Iñaki", "Unai",
                                                       "Victoriano", "Pedro Antonio", "Carlos Alberto", "Carles",
                                                       "Elias", "Jose Enrique", "Jeronimo", "Marco Antonio",
                                                       "Angel Luis", "Pol", "Juan Pablo", "Teodoro", "Matias", "Isidoro"
                                                       , "Dionisio", "Juan Ignacio", "Dario", "Arnau", "Roger",
                                                       "Candido", "Florencio", "Kevin", "Justo", "Blas",
                                                       "Francisco Jesus", "Roman", "Gustavo", "Santos"
                                                     };

      /// <summary>
      /// Nombres de hembra
      /// </summary>
      public static readonly string[] NombresHembra = new[]
                                                       {
                                                         "Maria Carmen", "Maria", "Carmen", "Josefa", "Isabel",
                                                         "Ana Maria", "Maria Dolores", "Maria Pilar", "Maria Teresa",
                                                         "Ana", "Francisca", "Laura", "Antonia", "Dolores",
                                                         "Maria Angeles", "Cristina", "Marta", "Maria Jose",
                                                         "Maria Isabel", "Pilar", "Maria Luisa", "Concepcion", "Lucia",
                                                         "Mercedes", "Manuela", "Elena", "Rosa Maria", "Raquel",
                                                         "Maria Jesus", "Sara", "Juana", "Teresa", "Rosario", "Paula",
                                                         "Encarnacion", "Beatriz", "Rosa", "Nuria", "Silvia",
                                                         "Montserrat", "Patricia", "Julia", "Monica", "Irene",
                                                         "Margarita", "Andrea", "Maria Mar", "Angela", "Rocio", "Sonia",
                                                         "Susana", "Sandra", "Alicia", "Maria Josefa", "Yolanda",
                                                         "Marina", "Alba", "Natalia", "Maria Rosario", "Inmaculada",
                                                         "Angeles", "Esther", "Maria Mercedes", "Ana Isabel", "Eva",
                                                         "Amparo", "Veronica", "Noelia", "Maria Rosa", "Maria Victoria",
                                                         "Maria Concepcion", "Consuelo", "Catalina", "Carolina",
                                                         "Eva Maria", "Victoria", "Maria Antonia", "Lorena", "Ana Belen"
                                                         , "Maria Elena", "Claudia", "Emilia", "Luisa", "Miriam", "Ines"
                                                         , "Nerea", "Maria Nieves", "Gloria", "Lidia", "Aurora",
                                                         "Josefina", "Esperanza", "Milagros", "Olga", "Carla",
                                                         "Purificacion", "Maria Soledad", "Sofia", "Celia",
                                                         "Maria Cristina", "Maria Luz", "Virginia", "Lourdes", "Fatima",
                                                         "Vanesa", "Magdalena", "Vicenta", "Begoña", "Asuncion", "Clara"
                                                         , "Anna", "Matilde", "Alejandra", "Remedios", "Elisa",
                                                         "Isabel Maria", "Estefania", "Maria Belen", "Trinidad",
                                                         "Araceli", "Maria Asuncion", "Elvira", "Maria Paz", "Natividad"
                                                         , "Soledad", "Maria Begoña", "Ainhoa", "Felisa", "Belen",
                                                         "Gema", "Maria Esther", "Maria Lourdes", "Ascension", "Blanca",
                                                         "Vanessa", "Tamara", "Nieves", "Maria Cruz", "Rafaela", "Gemma"
                                                         , "Paloma", "Adela", "Almudena", "Rebeca", "Daniela", "Ramona",
                                                         "Amalia", "Maria Amparo", "Maria Inmaculada", "Amelia", "Noemi"
                                                         , "Maria Eugenia", "Adriana", "Mireia", "Joaquina", "Tania",
                                                         "Jessica", "Juana Maria", "Petra", "Leonor", "Juliana",
                                                         "Carmen Maria", "Maria Rocio", "Guadalupe", "Agustina",
                                                         "Mariana", "Laia", "Diana", "Barbara", "Rosalia", "Martina",
                                                         "Cecilia", "Leticia", "Adoracion", "Elisabet",
                                                         "Maria Encarnacion", "Maria Magdalena", "Maria Francisca",
                                                         "Jennifer", "Estrella", "Judith", "Ester", "Ariadna", "Carlota"
                                                         , "Sheila", "Eugenia", "Judit", "Maria Gloria",
                                                         "Maria Milagros", "Maria Consuelo", "Valentina", "Herminia",
                                                         "Eulalia", "Ruth", "Soraya", "Enriqueta", "Maria Montserrat",
                                                         "Lara", "Maria Yolanda", "Leire"
                                                       };


      /// <summary>
      /// Apellidos más comunes
      /// </summary>
      public static readonly string[] Apellidos = new[]
                                                        {
                                                          "Garcia", "Gonzalez", "Rodriguez", "Fernandez", "Lopez",
                                                          "Martinez", "Sanchez", "Perez", "Gomez", "Martin", "Jimenez",
                                                          "Ruiz", "Hernandez", "Diaz", "Moreno", "Alvarez", "Muñoz",
                                                          "Romero", "Alonso", "Gutierrez", "Navarro", "Torres",
                                                          "Dominguez", "Vazquez", "Ramos", "Gil", "Ramirez", "Serrano",
                                                          "Blanco", "Suarez", "Molina", "Morales", "Ortega", "Delgado",
                                                          "Castro", "Ortiz", "Rubio", "Marin", "Sanz", "Iglesias",
                                                          "Nuñez", "Medina", "Garrido", "Santos", "Castillo", "Cortes",
                                                          "Lozano", "Guerrero", "Cano", "Prieto", "Mendez", "Calvo",
                                                          "Gallego", "Vidal", "Cruz", "Leon", "Herrera", "Marquez",
                                                          "Peña", "Cabrera", "Flores", "Campos", "Vega", "Diez",
                                                          "Fuentes", "Carrasco", "Caballero", "Nieto", "Aguilar",
                                                          "Pascual", "Reyes", "Herrero", "Santana", "Lorenzo", "Hidalgo"
                                                          , "Montero", "Ibañez", "Gimenez", "Ferrer", "Duran", "Vicente"
                                                          , "Benitez", "Mora", "Arias", "Santiago", "Vargas", "Carmona",
                                                          "Crespo", "Pastor", "Roman", "Soto", "Saez", "Velasco",
                                                          "Soler", "Moya", "Esteban", "Parra", "Bravo", "Gallardo",
                                                          "Rojas", "Pardo", "Merino", "Franco", "Izquierdo", "Espinosa",
                                                          "Lara", "Rivas", "Silva", "Casado", "Rivera", "Redondo",
                                                          "Arroyo", "Rey", "Camacho", "Vera", "Otero", "Galan", "Luque",
                                                          "Montes", "Rios", "Sierra", "Segura", "Carrillo", "Marcos",
                                                          "Marti", "Soriano", "Mendoza", "Robles", "Bernal", "Vila",
                                                          "Valero", "Palacios", "Exposito", "Pereira", "Benito",
                                                          "Andres", "Varela", "Guerra", "Macias", "Bueno", "Heredia",
                                                          "Roldan", "Mateo", "Villar", "Contreras", "Miranda", "Guillen"
                                                          , "Mateos", "Escudero", "Menendez", "Aguilera", "Casas",
                                                          "Aparicio", "Rivero", "Estevez", "Beltran", "Padilla",
                                                          "Gracia", "Rico", "Calderon", "Abad", "Galvez", "Conde",
                                                          "Salas", "Quintana", "Jurado", "Plaza", "Acosta", "Aranda",
                                                          "Blazquez", "Roca", "Costa", "Bermudez", "Miguel",
                                                          "Santamaria", "Salazar", "Serra", "Guzman", "Villanueva",
                                                          "Cuesta", "Manzano", "Tomas", "Hurtado", "Rueda", "Trujillo",
                                                          "Simon", "Pacheco", "Avila", "Pons", "De La Fuente", "Lazaro",
                                                          "Sancho", "Mesa", "Del Rio", "Escobar", "Blasco", "Millan",
                                                          "Alarcon", "Luna", "Castaño", "Zamora", "Salvador", "Bermejo",
                                                          "Paredes", "Anton", "Ballesteros", "Valverde", "Maldonado",
                                                          "Valle", "Bautista", "Ponce", "Rodrigo", "Lorente", "Juan",
                                                          "Oliva", "Mas", "Cordero", "Collado", "Pozo", "Murillo",
                                                          "Cuenca", "De La Cruz", "Montoya", "Martos", "Cuevas", "Marco"
                                                          , "Barroso", "Ros", "Quesada", "De La Torre", "Barrera",
                                                          "Ordoñez", "Gimeno", "Corral", "Alba", "Puig", "Cabello",
                                                          "Rojo", "Saiz", "Pulido", "Navas", "Aguado", "Soria", "Arenas"
                                                          , "Domingo", "Galindo", "Escribano", "Vallejo", "Mena",
                                                          "Asensio", "Ramon", "Valencia", "Lucas", "Caro", "Polo",
                                                          "Aguirre", "Naranjo", "Mata", "Villalba", "Paz", "Reina",
                                                          "Moran", "Linares", "Amador", "Ojeda", "Leal", "Burgos",
                                                          "Oliver", "Carretero", "Bonilla", "Sosa", "Roig", "Aragon",
                                                          "Carrion", "Clemente", "Chen", "Villa", "Castellano",
                                                          "Carrera", "Hernando", "Rosa", "Andreu", "Cordoba", "Caceres",
                                                          "Ferreira", "Calero", "Correa", "Cobo", "Cardenas", "Juarez",
                                                          "Domenech", "Alcaraz", "Velazquez", "Riera", "Sola", "Mohamed"
                                                          , "Chacon", "Llorente", "Saavedra", "Zapata", "Toledo",
                                                          "Moral", "Vela", "Salgado", "Carbonell", "Arribas", "Villegas"
                                                          , "Prado", "Alfonso", "Pelaez", "Requena", "Sevilla", "Font",
                                                          "Ayala", "Luis", "Carballo", "Piñeiro", "Olivares", "Da Silva"
                                                          , "Barrios", "Marques", "Esteve", "Solis", "Pinto", "Grau",
                                                          "Salinas", "Quintero", "Bosch", "Camara", "Perea", "Cid",
                                                          "Pineda", "Marrero", "Ballester", "Castilla", "Cantero",
                                                          "Sanchis", "Palomo", "Arevalo", "De La Rosa", "Sala",
                                                          "Casanova", "Miralles", "Rincon", "Lago", "Nicolas", "Baena",
                                                          "Herranz", "Belmonte", "Porras", "Arranz", "Cardona", "Recio",
                                                          "Muñiz", "Pino", "Palma", "Barba", "Coll", "Ventura",
                                                          "Barreiro", "Cobos", "Cabezas", "Cuadrado", "Cervera",
                                                          "Angulo", "Velez", "Puente", "Madrid", "Vaquero", "Becerra",
                                                          "Ochoa", "Pujol", "Ocaña", "Navarrete", "Tapia", "Granados",
                                                          "Valls", "Bello", "Alfaro", "Vergara", "Singh", "Latorre",
                                                          "Losada", "Mejias", "Rovira", "Campo", "Peralta", "Gamez",
                                                          "Sastre", "Egea", "Corrales", "Castellanos", "Falcon",
                                                          "Catalan", "Barragan", "Fraile", "Alcantara", "Cebrian",
                                                          "Estrada", "Godoy"
                                                        };
    }
  }
}
