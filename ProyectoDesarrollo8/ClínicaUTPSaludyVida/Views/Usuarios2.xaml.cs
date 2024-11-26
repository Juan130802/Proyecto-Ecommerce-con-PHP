using Clínica_UTP_Salud_y_Vida.Models;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class Usuarios2 : ContentPage
    {
        private readonly LocalDbService _dbService;
        private readonly int _editUsuarioId;

        public Usuarios2(LocalDbService dbService, Usuarios cita = null)
        {
            InitializeComponent();
            _dbService = dbService;

            if (cita != null)
            {
                _editUsuarioId = cita.Id;
                NameEntryField.Text = cita.name;
                TefelonoEntryField.Text = cita.telefono;
                TipoConsultaEntryField.SelectedItem = cita.tipo_consulta;
                FechaEntryField.Date = cita.Fecha;
                HoraEntryField.Time = cita.Hora;
                MotivoEntryField.Text = cita.motivo;
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntryField.Text) ||
                string.IsNullOrWhiteSpace(TefelonoEntryField.Text) ||
                TipoConsultaEntryField.SelectedItem == null ||
                string.IsNullOrWhiteSpace(MotivoEntryField.Text))
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "Aceptar");
                return;
            }

            DateTime fechaSeleccionada = FechaEntryField.Date;
            TimeSpan horaSeleccionada = HoraEntryField.Time;
            string tipoConsultaSeleccionada = (string)TipoConsultaEntryField.SelectedItem;

            var citaExistente = await _dbService.GetCitaPorHoraYTipoConsulta(fechaSeleccionada, horaSeleccionada, tipoConsultaSeleccionada);
            if (citaExistente != null && _editUsuarioId <= 0)
            {
                await DisplayAlert("Error", "Ya existe una cita para este tipo de consulta en la misma fecha y hora. Por favor, elija otra hora.", "Aceptar");
                return;
            }

            string idPacienteStr = Preferences.Get("IdPaciente", "0");
            if (!int.TryParse(idPacienteStr, out int idPaciente))
            {
                await DisplayAlert("Error", "El ID del paciente no es válido.", "Aceptar");
                return;
            }

            var usuario = new Usuarios
            {
                name = NameEntryField.Text,
                telefono = TefelonoEntryField.Text,
                tipo_consulta = tipoConsultaSeleccionada,
                Fecha = fechaSeleccionada,
                Hora = horaSeleccionada,
                motivo = MotivoEntryField.Text,
                IdPaciente = idPaciente
            };

            try
            {
                if (_editUsuarioId > 0)
                {
                    usuario.Id = _editUsuarioId;
                    await _dbService.Update(usuario);
                    await DisplayAlert("Citas", $"Cita Actualizada Correctamente", "Ok");
                    await Navigation.PushAsync(new CitasAgendadasUsuario(new LocalDbService()));
                }
                else
                {
                    await _dbService.Create(usuario);
                    await DisplayAlert("Citas", $"Cita Agendada Correctamente", "Ok");
                    await Navigation.PushAsync(new CitasAgendadasUsuario(new LocalDbService()));
                }

                NameEntryField.Text = string.Empty;
                TefelonoEntryField.Text = string.Empty;
                TipoConsultaEntryField.SelectedItem = null;
                FechaEntryField.Date = DateTime.Now;
                HoraEntryField.Time = TimeSpan.Zero;
                MotivoEntryField.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al guardar: {ex.Message}", "OK");
            }
        }
    }
}
