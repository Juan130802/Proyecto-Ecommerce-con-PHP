using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using SQLite;
using Image = iText.Layout.Element.Image;
using TextAlignment = iText.Layout.Properties.TextAlignment;


namespace Clínica_UTP_Salud_y_Vida.Models
{
    public class LocalDbService
    {
        private const string DB_NAME = "demo_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<PacientesRegister>().Wait();
            _connection.CreateTableAsync<Doctores>().Wait();
            _connection.CreateTableAsync<Usuarios>().Wait();
            _connection.CreateTableAsync<Recetas>().Wait();
            _connection.CreateTableAsync<Nurse>().Wait();
            _connection.CreateTableAsync<PacienteHistorial>().Wait();
            _connection.CreateTableAsync<Customer>().Wait();
            _connection.CreateTableAsync<Donantes>().Wait();
            _connection.CreateTableAsync<Laboratorista>().Wait();
            _connection.CreateTableAsync<ProcedimientoMedico>().Wait();
            _connection.CreateTableAsync<Referencia>().Wait();
        }

        public async Task<int> CreateReferencia(Referencia referencia)
        {
            await _connection.InsertAsync(referencia);
            return referencia.id;
        }

        public async Task<Referencia> GetPorReferencia(int Id)
        {
            return await _connection.Table<Referencia>()
                .FirstOrDefaultAsync(r => r.id == Id);
        }

        public async Task<List<Referencia>> GetTodasLasReferencias()
        {
            return await _connection.Table<Referencia>().ToListAsync();
        }



        public async Task<int> CreateLab(Laboratorista lab)
        {
            return await _connection.InsertAsync(lab);
        }

        public async Task<int> UpdateLab(Laboratorista lab)
        {
            return await _connection.UpdateAsync(lab);
        }

        public async Task<List<Laboratorista>> GetLabs()
        {
            return await _connection.Table<Laboratorista>().ToListAsync();
        }


        public async Task<List<Donantes>> GetDonantes()
        {
            return await _connection.Table<Donantes>().ToListAsync();
        }

        // Obtiene un donante específico por cédula
        public Task<Donantes> ObtenerDonantePorCedula(string cedula)
        {
            return _connection.Table<Donantes>().FirstOrDefaultAsync(d => d.Cedula == cedula);
        }

        // Busca donantes por cédula
        public async Task<List<Donantes>> BuscarDonantesPorCedula(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return new List<Donantes>();
            }

            var donantes = await _connection.Table<Donantes>()
                .Where(d => d.Cedula.Contains(cedula)) // Permite coincidencias parciales
                .ToListAsync();

            return donantes;
        }

        public async Task<List<Donantes>> ObtenerDonantesConEstadoEnEspera()
        {
            return await _connection.Table<Donantes>()
                                    .Where(d => d.Status == "En Espera")
                                    .ToListAsync();
        }

        public async Task<Donantes> ObtenerDonantePorId(int donanteId)
        {
            return await _connection.Table<Donantes>()
                .Where(d => d.Id == donanteId)
                .FirstOrDefaultAsync();
        }


        // Inserta un nuevo donante en la base de datos
        public Task<int> Insert(Donantes donante)
        {
            return _connection.InsertAsync(donante);
        }

        // Elimina un donante de la base de datos
        public Task<int> Delete(Donantes donante)
        {
            return _connection.DeleteAsync(donante);
        }

        // Actualiza los datos de un donante
        public Task<int> Update(Donantes donante)
        {
            return _connection.UpdateAsync(donante);
        }

        public async Task<int> CreateCustomer(Customer customer)
        {
            return await _connection.InsertAsync(customer);
        }

        // Método para guardar un historial médico
        public async Task<int> GuardarHistorialMedico(PacienteHistorial historial)
        {
            return await _connection.InsertAsync(historial);
        }

        // Método para obtener el historial médico de un paciente por ID
        public async Task<PacienteHistorial> ObtenerHistorialPorId(int pacienteId)
        {
            return await _connection.Table<PacienteHistorial>()
                .Where(h => h.PacienteId == pacienteId)
                .FirstOrDefaultAsync();
        }

        // Método para actualizar un historial médico
        public async Task<int> ActualizarHistorialMedico(PacienteHistorial historial)
        {
            return await _connection.UpdateAsync(historial);
        }

        // Método para eliminar un historial médico
        public async Task<int> EliminarHistorialMedico(int id)
        {
            return await _connection.DeleteAsync<PacienteHistorial>(id);
        }

        // Método para obtener todos los historiales médicos
        public async Task<List<PacienteHistorial>> ObtenerTodosLosHistoriales()
        {
            return await _connection.Table<PacienteHistorial>().ToListAsync();
        }


        public async Task<List<PacientesRegister>> BuscarPorCedula(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return new List<PacientesRegister>();
            }
            var pacientes = await _connection.Table<PacientesRegister>()
                .Where(p => p.Cedula == cedula)
                .ToListAsync();

            return pacientes;
        }

        // Métodos CRUD para Recetas
        public async Task CreateReceta(Recetas receta)
        {
            await _connection.InsertAsync(receta);
        }

        public async Task<List<Recetas>> GetRecetasPorPaciente(int idPaciente)
        {
            return await _connection.Table<Recetas>()
                .Where(r => r.IdPaciente == idPaciente)
                .ToListAsync();
        }

        private async Task<Customer> GetCustomerById(int idCustomerEspecifico)
        {
            return await _connection.Table<Customer>()
                .Where(c => c.Id == idCustomerEspecifico)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateReceta(Recetas receta)
        {
            await _connection.UpdateAsync(receta);
        }

        public async Task DeleteReceta(Recetas receta)
        {
            await _connection.DeleteAsync(receta);
        }

        public async Task<bool> ValidateAdmin(string usuario, string password)
        {
            return usuario == "Admin" && password == "12345JJJJA";
        }

        // Obtener todas las citas (usuarios)
        public async Task<List<Usuarios>> GetUsuarios()
        {
            return await _connection.Table<Usuarios>().ToListAsync();
        }

        // Obtener citas por ID de paciente
        public async Task<List<Usuarios>> GetCitasPorIdPaciente(int idPaciente)
        {
            return await _connection.Table<Usuarios>()
                .Where(u => u.IdPaciente == idPaciente)
                .ToListAsync();
        }

        // Obtener citas por ID de doctor
        public async Task<List<Usuarios>> GetCitasPorDoctorId(int doctorId)
        {
            return await _connection.Table<Usuarios>()
                .Where(u => u.Id == doctorId)
                .ToListAsync();
        }


        // Obtener cita por fecha, hora y tipo de consulta (método para evitar duplicidad)
        public async Task<Usuarios> GetCitaPorHoraYTipoConsulta(DateTime fecha, TimeSpan hora, string tipoConsulta)
        {
            return await _connection.Table<Usuarios>()
                                      .Where(u => u.Fecha == fecha && u.Hora == hora && u.tipo_consulta == tipoConsulta)
                                      .FirstOrDefaultAsync();
        }

        // Métodos CRUD para Usuarios (Citas)
        public async Task Create(Usuarios usuarios)
        {
            await _connection.InsertAsync(usuarios);
        }

        public async Task Update(Usuarios usuarios)
        {
            // Asegúrate de que el registro siempre se actualice, independientemente de los valores de DoctorId o DoctorAsignado
            await _connection.UpdateAsync(usuarios);
        }


        public async Task AsignarDoctorSiNoExiste(int citaId, Doctores doctor)
        {
            var cita = await _connection.Table<Usuarios>().Where(u => u.Id == citaId).FirstOrDefaultAsync();
            if (cita != null && (cita.DoctorId == 0 || string.IsNullOrWhiteSpace(cita.DoctorAsignado)))
            {
                cita.DoctorId = doctor.Id;
                cita.DoctorAsignado = doctor.Name;
                await _connection.UpdateAsync(cita);
            }
        }

        public async Task Delete(Usuarios usuario)
        {
            await _connection.DeleteAsync(usuario);
        }

        // Obtener todos los registros de pacientes
        public async Task<List<PacientesRegister>> GetPacienteRegis()
        {
            return await _connection.Table<PacientesRegister>().ToListAsync();
        }

        public async Task CreateProcedimiento(ProcedimientoMedico procedimiento)
        {
            await _connection.InsertAsync(procedimiento);
        }
        public async Task<List<ProcedimientoMedicoExtendido>> ObtenerTodosLosProcedimientos()
        {
            var procedimientos = await _connection.Table<ProcedimientoMedico>().ToListAsync();
            Console.WriteLine($"Procedimientos encontrados: {procedimientos.Count}");

            var pacientes = await _connection.Table<PacientesRegister>().ToListAsync();
            Console.WriteLine($"Pacientes encontrados: {pacientes.Count}");

            var procedimientosExtendidos = new List<ProcedimientoMedicoExtendido>();

            foreach (var procedimiento in procedimientos)
            {
                var paciente = pacientes.FirstOrDefault(p => p.Id == procedimiento.PacienteId);
                if (paciente != null)
                {
                    procedimientosExtendidos.Add(new ProcedimientoMedicoExtendido
                    {
                        Id = procedimiento.Id,
                        TipoProcedimiento = procedimiento.TipoProcedimiento,
                        Fecha = procedimiento.Fecha,
                        Observaciones = procedimiento.Observaciones,
                        NombrePaciente = paciente.Name,
                        CedulaPaciente = paciente.Cedula
                    });
                }
                else
                {
                    Console.WriteLine($"No se encontró paciente para el procedimiento con ID {procedimiento.Id}");
                }
            }

            Console.WriteLine($"Procedimientos extendidos encontrados: {procedimientosExtendidos.Count}");
            return procedimientosExtendidos;
        }

        public async Task<List<ProcedimientoMedicoExtendido>> ObtenerProcedimientosDelDia()
        {
            var procedimientos = await _connection.Table<ProcedimientoMedico>().ToListAsync();
            var pacientes = await _connection.Table<PacientesRegister>().ToListAsync();

            var procedimientosDelDia = procedimientos
                .Where(p => p.Fecha.Date == DateTime.Today)
                .Select(p => new ProcedimientoMedicoExtendido
                {
                    Id = p.Id,
                    TipoProcedimiento = p.TipoProcedimiento,
                    Fecha = p.Fecha,
                    Observaciones = p.Observaciones,
                    NombrePaciente = pacientes.FirstOrDefault(pa => pa.Id == p.PacienteId).Name,
                    CedulaPaciente = pacientes.FirstOrDefault(pa => pa.Id == p.PacienteId).Cedula
                })
                .ToList();

            return procedimientosDelDia;
        }

        public async Task GenerarReporteDiario()
        {
            var procedimientosDelDia = await ObtenerProcedimientosDelDia();

            if (!procedimientosDelDia.Any())
            {
                await Application.Current.MainPage.DisplayAlert("Reporte Diario", "No se realizaron procedimientos hoy.", "OK");
                return;
            }

            // Crear la ruta del archivo
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
            var ruta = Path.Combine(@"D:\HP\Nueva carpeta\Juan Tareas", $"ReporteDiario-{fechaHoy}.pdf");

            using (var writer = new PdfWriter(ruta))
            using (var pdf = new PdfDocument(writer))
            using (var document = new Document(pdf))
            {
                document.Add(new Paragraph("Reporte Diario de Procedimientos Médicos")
                    .SetFontSize(20)
                    .SetBold());

                document.Add(new Paragraph($"Fecha: {DateTime.Now.ToShortDateString()}")
                    .SetFontSize(14)
                    .SetBold());

                document.Add(new Paragraph("\n")); // Espacio adicional

                foreach (var procedimiento in procedimientosDelDia)
                {
                    document.Add(new Paragraph($"Tipo: {procedimiento.TipoProcedimiento}"));
                    document.Add(new Paragraph($"Fecha: {procedimiento.Fecha}"));
                    document.Add(new Paragraph($"Paciente: {procedimiento.NombrePaciente}"));
                    document.Add(new Paragraph($"Cédula: {procedimiento.CedulaPaciente}"));
                    document.Add(new Paragraph($"Observaciones: {procedimiento.Observaciones}"));
                    document.Add(new Paragraph("\n")); // Espacio adicional entre procedimientos
                }

                document.Close();
            }

            await Application.Current.MainPage.DisplayAlert("Reporte Diario", $"Reporte generado exitosamente en: {ruta}", "OK");
        }

        public async Task<List<PacientesRegister>> BuscarPorCedula1(string cedula)
        {
            return await _connection.Table<PacientesRegister>().Where(p => p.Cedula.Contains(cedula)).ToListAsync();
        }

        // Obtener paciente por ID
        public async Task<PacientesRegister> GetPacienteById(int id)
        {
            return await _connection.Table<PacientesRegister>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        // Métodos CRUD para PacientesRegister
        public async Task Create(PacientesRegister register)
        {
            await _connection.InsertAsync(register);
        }

        public async Task Update(PacientesRegister paciente)
        {
            var result = await _connection.UpdateAsync(paciente);
            if (result != 1) // Verifica que se haya actualizado un registro
            {
                throw new Exception("Error al actualizar el paciente");
            }
        }

        public async Task Delete(PacientesRegister register)
        {
            await _connection.DeleteAsync(register);
        }

        // Obtener todos los doctores
        public async Task<List<Doctores>> GetDoctores()
        {
            return await _connection.Table<Doctores>().ToListAsync();
        }

        // Métodos CRUD para Doctores
        public async Task Create(Doctores doctores)
        {
            await _connection.InsertAsync(doctores);
        }

        public async Task Update(Doctores doctores)
        {
            await _connection.UpdateAsync(doctores);
        }

        public async Task Delete(Doctores doctores)
        {
            await _connection.DeleteAsync(doctores);
        }

        // Métodos CRUD para Nurse

        public async Task<List<Nurse>> GetNurse()
        {
            return await _connection.Table<Nurse>().ToListAsync();
        }

        public async Task Create(Nurse nurse)
        {
            await _connection.InsertAsync(nurse);
        }
        public async Task Update(Nurse nurse)
        {
            await _connection.UpdateAsync(nurse);
        }
        public async Task Delete(Nurse nurse)
        {
            await _connection.DeleteAsync(nurse);
        }

        public async Task<string> GenerarCertificadoPDF(int idPaciente, string ruta, int idCustomerEspecifico)
        {
            var customerEspecifico = await GetCustomerById(idCustomerEspecifico);

            if (customerEspecifico == null)
            {
                throw new Exception("No hay recetas para este id.");
            }
            try
            {
                using (var writer = new PdfWriter(ruta))
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

                    string rutaImagenDerecha = Path.Combine(@"C:\Users\HP\source\repos\ClínicaUTPSaludyVida\Resources\Images", "logo.png");

                    float anchoImagen = 100;
                    float alturaImagen = 100;

                    float posicionVertical = pdf.GetDefaultPageSize().GetHeight() - 100;

                    Image imgRight = new Image(ImageDataFactory.Create(rutaImagenDerecha))
                        .SetFixedPosition(pdf.GetDefaultPageSize().GetWidth() - anchoImagen - 20, posicionVertical)
                        .SetWidth(anchoImagen)
                        .SetHeight(alturaImagen);

                    document.Add(imgRight);

                    // Encabezado
                    document.Add(new Paragraph("\n"));
                    document.Add(new Paragraph("Clínica UTP Salud y Vida")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBold()
                        .SetFontSize(16));
                    document.Add(new Paragraph("Dirección: Avenida UTP, Edificio A, Panamá")
                        .SetTextAlignment(TextAlignment.CENTER));
                    document.Add(new Paragraph("Teléfono: (+507) 1234-5678")
                        .SetTextAlignment(TextAlignment.CENTER));
                    document.Add(new Paragraph("\n"));

                    // Título del certificado
                    document.Add(new Paragraph("Certificado De Buena Salud")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(20));
                    document.Add(new Paragraph("\n"));

                    // Texto del certificado con resaltado para Nombre y Cédula en negrita
                    var certificadoTexto = new Paragraph("El Suscrito médico certifica que la SRA (SRO) ")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(12);

                    certificadoTexto.Add(new Text($"{customerEspecifico.Nombre}").SetBold());
                    certificadoTexto.Add(" , portador (a) del documento de identidad ");
                    certificadoTexto.Add(new Text($"{customerEspecifico.Cedula}").SetBold());
                    certificadoTexto.Add(" , se encuentra en buen estado de salud y no es portador (a) de enfermedades infectocontagiosas o de otro tipo que pueda poner en riesgo la salud pública, de conformidad con lo dispuesto en el Reglamento de la Clínica y del Ministerio de Salud.");

                    document.Add(certificadoTexto);

                    document.Add(new Paragraph("\n"));

                    document.Add(new Paragraph($"Se expide el presente certificado el: {DateTime.Now.ToString("d")}")
                        .SetTextAlignment(TextAlignment.CENTER));
                    document.Add(new Paragraph("\n\n"));

                    // Firma del doctor
                    document.Add(new Paragraph("Firma del doctor:")
                        .SetTextAlignment(TextAlignment.LEFT));
                    document.Add(new Paragraph("---------------------------------------------------")
                        .SetTextAlignment(TextAlignment.CENTER));
                    document.Add(new Paragraph("\n\n"));

                    // Información de contacto
                    document.Add(new Paragraph("© 2024 Clínica Universitaria")
                        .SetFontSize(14)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(ColorConstants.BLACK));
                    document.Add(new Paragraph("Contáctenos: +507 6254-2854 | clinica.universitaria@utp.ac.pa")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(ColorConstants.BLACK));
                    document.Add(new Paragraph("\n")); // Espacio al final

                    document.Close();
                }
                return ruta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el PDF: {ex.Message}");
            }
        }

        public async Task<string> GenerarPdfRecetas(int idPaciente, string ruta, int idRecetaEspecifica)
        {
            var recetas = await GetRecetasPorPaciente(idPaciente);
            var recetaEspecifica = recetas.FirstOrDefault(r => r.Id == idRecetaEspecifica);

            if (recetaEspecifica == null)
            {
                throw new Exception("No hay recetas para este id.");
            }

            try
            {
                using (var writer = new PdfWriter(ruta))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        string rutaImagenIzquierda = Path.Combine(@"C:\Users\HP\source\repos\ClínicaUTPSaludyVida\Resources\Images", "logoutp.png");
                        string rutaImagenDerecha = Path.Combine(@"C:\Users\HP\source\repos\ClínicaUTPSaludyVida\Resources\Images", "logo.png");

                        if (!File.Exists(rutaImagenIzquierda) || !File.Exists(rutaImagenDerecha))
                        {
                            throw new Exception("Una o ambas imágenes no se encontraron en la ruta especificada.");
                        }

                        float anchoImagen = 100;
                        float alturaImagen = 100;

                        float posicionVertical = pdf.GetDefaultPageSize().GetHeight() - 100;

                        Image imgLeft = new Image(ImageDataFactory.Create(rutaImagenIzquierda))
                            .SetFixedPosition(20, posicionVertical)
                            .SetWidth(anchoImagen)
                            .SetHeight(alturaImagen);

                        Image imgRight = new Image(ImageDataFactory.Create(rutaImagenDerecha))
                            .SetFixedPosition(pdf.GetDefaultPageSize().GetWidth() - anchoImagen - 20, posicionVertical)
                            .SetWidth(anchoImagen)
                            .SetHeight(alturaImagen);

                        document.Add(imgLeft);
                        document.Add(imgRight);

                        document.Add(new Paragraph("\n"));

                        // Encabezado
                        document.Add(new Paragraph("Clínica UTP Salud y Vida")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBold()
                            .SetFontSize(16));
                        document.Add(new Paragraph("Dirección: Avenida UTP, Edificio A, Panamá")
                            .SetTextAlignment(TextAlignment.CENTER));
                        document.Add(new Paragraph("Teléfono: (+507) 1234-5678")
                            .SetTextAlignment(TextAlignment.CENTER));
                        document.Add(new Paragraph($"Fecha: {DateTime.Now.ToString("d")}")
                            .SetTextAlignment(TextAlignment.CENTER));
                        document.Add(new Paragraph("---------------------------------------------------")
                            .SetTextAlignment(TextAlignment.CENTER));

                        // Información del doctor y paciente
                        document.Add(new Paragraph($"Doctor: {recetaEspecifica.NombreDoctor}"));
                        document.Add(new Paragraph($"Especialidad: {recetaEspecifica.Especialidad}"));
                        document.Add(new Paragraph($"Paciente: {recetaEspecifica.NombrePaciente}"));
                        document.Add(new Paragraph("---------------------------------------------------"));

                        // Encabezado de medicamentos
                        document.Add(new Paragraph("MEDICAMENTOS RECETADOS")
                            .SetBold().SetFontSize(14));
                        document.Add(new Paragraph("---------------------------------------------------"));
                        document.Add(new Paragraph("| Medicamento    | Dosis           |")
                            .SetTextAlignment(TextAlignment.LEFT));
                        document.Add(new Paragraph("---------------------------------------------------"));

                        // Agregar los medicamentos de la receta específica
                        document.Add(new Paragraph($"| {recetaEspecifica.Medicamento,-15} | {recetaEspecifica.Dosis,-15} |")
                            .SetTextAlignment(TextAlignment.LEFT));

                        document.Add(new Paragraph("---------------------------------------------------"));

                        // Información de contacto
                        document.Add(new Paragraph("© 2024 Clínica Universitaria")
                            .SetFontSize(14)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontColor(ColorConstants.BLACK));
                        document.Add(new Paragraph("Contáctenos: +507 6254-2854 | clinica.universitaria@utp.ac.pa")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontColor(ColorConstants.BLACK));
                        document.Add(new Paragraph("Síguenos en nuestras redes sociales")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontColor(ColorConstants.BLACK));

                        // Agregar imágenes de redes sociales
                        string rutaWhatsapp = Path.Combine(@"C:\Users\HP\source\repos\ClínicaUTPSaludyVida\Resources\Images", "whastapp.png");
                        string rutaInstagram = Path.Combine(@"C:\Users\HP\source\repos\ClínicaUTPSaludyVida\Resources\Images", "instagram.png");

                        Image imgWhatsapp = new Image(ImageDataFactory.Create(rutaWhatsapp))
                            .SetWidth(40)
                            .SetHeight(40);

                        Image imgInstagram = new Image(ImageDataFactory.Create(rutaInstagram))
                            .SetWidth(40)
                            .SetHeight(40);

                        // Crear un párrafo para las imágenes
                        var paragraph = new Paragraph()
                            .SetTextAlignment(TextAlignment.CENTER);

                        // Agregar las imágenes al párrafo
                        paragraph.Add(imgWhatsapp);
                        paragraph.Add(new Paragraph("   ")); // Espacio entre las imágenes
                        paragraph.Add(imgInstagram);

                        // Añadir el párrafo de imágenes al documento
                        document.Add(paragraph);

                        document.Close();
                    }
                }
                return ruta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el PDF: {ex.Message}");
            }
        }
        public async Task<string> GenerarPdfReferenciaMedica(int idReferenciaEspecifica, string ruta)
        {
            // Obtener la referencia específica
            var referencia = await GetPorReferencia(idReferenciaEspecifica);
            if (referencia == null)
            {
                throw new Exception("No hay referencia para este ID.");
            }

            try
            {
                using (var writer = new PdfWriter(ruta))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        // Rutas de las imágenes
                        string rutaImagenIzquierda = Path.Combine(@"C:\Users\HP\source\repos\ClínicaUTPSaludyVida\Resources\Images", "logoutp.png");
                        string rutaImagenDerecha = Path.Combine(@"C:\Users\HP\source\repos\ClínicaUTPSaludyVida\Resources\Images", "logo.png");

                        // Verificar si las imágenes existen
                        if (!File.Exists(rutaImagenIzquierda) || !File.Exists(rutaImagenDerecha))
                        {
                            throw new Exception("Una o ambas imágenes no se encontraron en la ruta especificada.");
                        }

                        // Configuración de las imágenes
                        float anchoImagen = 100;
                        float alturaImagen = 100;
                        float posicionVertical = pdf.GetDefaultPageSize().GetHeight() - 100;

                        // Insertar imágenes
                        Image imgLeft = new Image(ImageDataFactory.Create(rutaImagenIzquierda))
                            .SetFixedPosition(20, posicionVertical)
                            .SetWidth(anchoImagen)
                            .SetHeight(alturaImagen);
                        Image imgRight = new Image(ImageDataFactory.Create(rutaImagenDerecha))
                            .SetFixedPosition(pdf.GetDefaultPageSize().GetWidth() - anchoImagen - 20, posicionVertical)
                            .SetWidth(anchoImagen)
                            .SetHeight(alturaImagen);

                        document.Add(imgLeft);
                        document.Add(imgRight);
                        document.Add(new Paragraph("\n"));

                        // Encabezado de la clínica
                        document.Add(new Paragraph("Clínica UTP Salud y Vida")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBold()
                            .SetFontSize(16));
                        document.Add(new Paragraph("Dirección: Avenida UTP, Edificio A, Panamá")
                            .SetTextAlignment(TextAlignment.CENTER));
                        document.Add(new Paragraph("Teléfono: (+507) 1234-5678")
                            .SetTextAlignment(TextAlignment.CENTER));
                        document.Add(new Paragraph($"Fecha: {DateTime.Now.ToString("d")}")
                            .SetTextAlignment(TextAlignment.CENTER));
                        document.Add(new Paragraph("---------------------------------------------------")
                            .SetTextAlignment(TextAlignment.CENTER));

                        // Información del paciente y referencia
                        document.Add(new Paragraph($"Paciente: {referencia.Name}"));
                        document.Add(new Paragraph($"Cédula: {referencia.Cedula}"));
                        document.Add(new Paragraph($"Contacto: {referencia.Contacto ?? "No especificado"}"));
                        document.Add(new Paragraph($"Motivo de la referencia: {referencia.Motivo ?? "No especificado"}"));
                        document.Add(new Paragraph($"Especialidad médica: {referencia.especialidadmedica ?? "No especificado"}"));
                        document.Add(new Paragraph("---------------------------------------------------"));

                        // Información de contacto adicional
                        document.Add(new Paragraph("© 2024 Clínica Universitaria")
                            .SetFontSize(14)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontColor(ColorConstants.BLACK));
                        document.Add(new Paragraph("Contáctenos: +507 6254-2854 | clinica.universitaria@utp.ac.pa")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontColor(ColorConstants.BLACK));
                        document.Add(new Paragraph("Síguenos en nuestras redes sociales")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFontColor(ColorConstants.BLACK));

                        // Agregar imágenes de redes sociales
                        string rutaWhatsapp = Path.Combine(@"C:\Users\HP\source\repos\ClínicaUTPSaludyVida\Resources\Images", "whastapp.png");
                        string rutaInstagram = Path.Combine(@"C:\Users\HP\source\repos\ClínicaUTPSaludyVida\Resources\Images", "instagram.png");

                        if (File.Exists(rutaWhatsapp) && File.Exists(rutaInstagram))
                        {
                            Image imgWhatsapp = new Image(ImageDataFactory.Create(rutaWhatsapp))
                                .SetWidth(40)
                                .SetHeight(40);
                            Image imgInstagram = new Image(ImageDataFactory.Create(rutaInstagram))
                                .SetWidth(40)
                                .SetHeight(40);

                            // Crear un párrafo para centrar las imágenes de redes sociales
                            var paragraph = new Paragraph().SetTextAlignment(TextAlignment.CENTER);
                            paragraph.Add(imgWhatsapp);
                            paragraph.Add(new Paragraph("   ")); // Espacio entre imágenes
                            paragraph.Add(imgInstagram);

                            document.Add(paragraph);
                        }
                        else
                        {
                            throw new Exception("Una o ambas imágenes de redes sociales no se encontraron.");
                        }

                        document.Close();
                    }
                }
                return ruta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar el PDF: {ex.Message}");
            }
        }

    }
}