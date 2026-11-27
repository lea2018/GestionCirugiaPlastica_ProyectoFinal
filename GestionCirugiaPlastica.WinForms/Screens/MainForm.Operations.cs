using System.Drawing;
using System.Globalization;
using System.Text.Json;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm
{
    private async Task EditPatient(ApiPaciente? current)
    {
        using var form = new EntityDialog(current is null ? "Registrar paciente" : "Editar paciente");
        var first = form.AddText("Nombre", current?.Nombre); var last = form.AddText("Apellido", current?.Apellido);
        var cedula = form.AddText("Cédula", current?.Cedula); var phone = form.AddText("Teléfono", current?.Telefono); var email = form.AddText("Correo", current?.Email);
        var address = form.AddText("Dirección", current?.Direccion); var sex = form.AddText("Sexo", current?.Sexo);
        var birth = form.AddDate("Fecha de nacimiento", current?.FechaNacimiento?.ToDateTime(TimeOnly.MinValue)); var insurance = form.AddCheck("Tiene seguro", current?.TieneSeguro ?? false);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        if (string.IsNullOrWhiteSpace(first.Text) || string.IsNullOrWhiteSpace(last.Text)) { MessageBox.Show(this, "Nombre y apellido son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        var payload = new { PacienteId = current?.PacienteId ?? 0, FechaRegistro = current?.FechaRegistro ?? DateTime.Now, Nombre = first.Text.Trim(), Apellido = last.Text.Trim(), Cedula = EmptyNull(cedula.Text), FechaNacimiento = birth.Checked ? birth.Value.Date.ToString("yyyy-MM-dd") : null, TieneSeguro = insurance.Checked, Sexo = EmptyNull(sex.Text), Telefono = EmptyNull(phone.Text), Email = EmptyNull(email.Text), Direccion = EmptyNull(address.Text) };
        await Run(async () => { await _api.SendAsync(current is null ? HttpMethod.Post : HttpMethod.Put, current is null ? "api/Pacientes" : $"api/Pacientes/{current.PacienteId}", payload); MessageBox.Show(this, "Paciente guardado correctamente.", "Tanger Clinic", MessageBoxButtons.OK, MessageBoxIcon.Information); await LoadSection("Pacientes"); });
    }

    private async Task EditAppointment(ApiCita? current)
    {
        try { _patients = await _api.GetAsync<List<ApiPaciente>>("api/Pacientes"); }
        catch (Exception ex) { MessageBox.Show(this, UserMessage(ex), "No se pudo cargar pacientes", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        using var form = new EntityDialog(current is null ? "Registrar cita" : "Editar cita");
        var patient = form.AddChoice("Paciente", _patients.Select(p => new ChoiceItem(p.PacienteId, $"{p.Nombre} {p.Apellido} · #{p.PacienteId}")).ToList(), current?.PacienteId);
        var date = form.AddDateTime("Fecha y hora", current?.Fecha ?? DateTime.Now.AddDays(1)); var type = form.AddText("Tipo de cita", current?.TipoCita);
        var cost = form.AddText("Costo", current?.Costo.ToString("0.00") ?? "0.00"); var state = form.AddChoiceText("Estado", ["Pendiente", "Confirmada", "Llegó", "Atendida", "Completada", "Cancelada"], current?.Estado ?? "Pendiente");
        var payment = form.AddChoiceText("Estado de pago", ["Pendiente", "Pagado", "Parcial"], current?.EstadoPago ?? "Pendiente"); var notes = form.AddText("Observaciones", current?.Observaciones, multiline: true);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        if (patient.SelectedItem is not ChoiceItem selected || !decimal.TryParse(cost.Text, out var amount)) { MessageBox.Show(this, "Selecciona un paciente e indica un costo válido.", "Validación"); return; }
        var payload = new { CitaId = current?.CitaId ?? 0, Fecha = date.Value, PacienteId = selected.Id, TipoCita = EmptyNull(type.Text), Costo = amount, Estado = state.Text, EstadoPago = payment.Text, Observaciones = EmptyNull(notes.Text), CitaAnteriorId = current?.CitaAnteriorId };
        await Run(async () => { await _api.SendAsync(current is null ? HttpMethod.Post : HttpMethod.Put, current is null ? "api/Citas" : $"api/Citas/{current.CitaId}", payload); MessageBox.Show(this, "Cita guardada correctamente.", "Tanger Clinic", MessageBoxButtons.OK, MessageBoxIcon.Information); await LoadSection("Citas"); });
    }

    private async Task EditSurgery(ApiCirugia? current, Func<Task>? refresh = null)
    {
        try
        {
            var patients = await _api.GetAsync<List<ApiPaciente>>("api/Pacientes/expediente/selector");
            var procedures = await _api.GetAsync<List<ApiProcedimiento>>("api/Procedimientos");
            if (patients.Count == 0)
            {
                MessageBox.Show(this, "El selector autorizado de pacientes no devolvió registros. No se puede asociar una cirugía sin un paciente.", "Sin pacientes disponibles", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (procedures.Count == 0)
            {
                if (MessageBox.Show(this, "El catálogo no tiene procedimientos. ¿Quieres agregar uno ahora (por ejemplo, Blefaroplastia)?", "Sin procedimientos disponibles", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes && await CreateProcedure())
                {
                    if (refresh is null) await LoadSection("Cirugías"); else await refresh();
                    await EditSurgery(null, refresh);
                }
                return;
            }
            using var form = new EntityDialog(current is null ? "Registrar cirugía" : "Editar cirugía");
            var patient = form.AddChoice("Paciente", patients.Select(p => new ChoiceItem(p.PacienteId, $"{p.Nombre} {p.Apellido} · #{p.PacienteId}")).ToList(), current?.PacienteId);
            var procedure = form.AddChoice("Tipo de cirugía / procedimiento", procedures.Select(p => new ChoiceItem(p.ProcedimientoId, p.Nombre)).ToList(), current?.ProcedimientoId);
            var date = form.AddDateTime("Fecha", current?.FechaCirugia ?? DateTime.Now);
            var state = form.AddChoiceText("Estado", ["Programada", "Pendiente", "Completada", "Cancelada"], current?.Estado ?? "Programada");
            var notes = form.AddText("Descripción del procedimiento / observaciones médicas", current?.Observaciones, true);
            if (form.ShowDialog(this) != DialogResult.OK) return;
            if (patient.SelectedItem is not ChoiceItem selectedPatient || procedure.SelectedItem is not ChoiceItem selectedProcedure) throw new InvalidOperationException("Selecciona el paciente y el procedimiento.");
            var payload = new { CirugiaId = current?.CirugiaId ?? 0, PacienteId = selectedPatient.Id, ProcedimientoId = selectedProcedure.Id, FechaCirugia = date.Value, Estado = state.Text, DiagnosticoPreoperatorio = current?.DiagnosticoPreoperatorio, Observaciones = EmptyNull(notes.Text) };
            await Run(async () => { await _api.SendAsync(current is null ? HttpMethod.Post : HttpMethod.Put, current is null ? "api/Cirugias" : $"api/Cirugias/{current.CirugiaId}", payload); MessageBox.Show(this, "Cirugía guardada.", "Tanger Clinic"); if (refresh is null) await LoadSection("Cirugías"); else await refresh(); });
        }
        catch (Exception ex) { MessageBox.Show(this, UserMessage(ex), "No se pudo abrir el formulario", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private async Task<bool> CreateProcedure()
    {
        var names = new[] { "Blefaroplastia", "Rinoplastia", "Abdominoplastia", "Liposucción", "Mamoplastia de aumento", "Mamoplastia de reducción", "Otoplastia", "Lifting facial", "Otro" };
        using var form = new EntityDialog("Agregar procedimiento al catálogo");
        var choice = form.AddChoiceText("Procedimiento", names, names[0]);
        var customName = form.AddText("Nombre personalizado (si seleccionaste Otro)");
        var description = form.AddText("Descripción", multiline: true);
        var price = form.AddText("Precio base (monto definido por la clínica)", "0.00");
        if (form.ShowDialog(this) != DialogResult.OK) return false;
        var name = choice.Text == "Otro" ? customName.Text.Trim() : choice.Text;
        if (string.IsNullOrWhiteSpace(name) || !decimal.TryParse(price.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var amount) || amount <= 0)
        {
            MessageBox.Show(this, "Indica el nombre y un precio base mayor que cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }
        try
        {
            await _api.SendAsync(HttpMethod.Post, "api/Procedimientos", new { ProcedimientoId = 0, FechaRegistro = DateTime.Now, Nombre = name, Descripcion = EmptyNull(description.Text), PrecioBase = amount });
            MessageBox.Show(this, "Procedimiento agregado al catálogo.", "Tanger Clinic", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
        catch (Exception ex) { MessageBox.Show(this, UserMessage(ex), "No se pudo agregar el procedimiento", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
    }

    private async Task EditPayment(ApiPago? current, ApiEstadoCuentaPago? pending = null)
    {
        try
        {
            var patients = await _api.GetAsync<List<ApiPaciente>>("api/Pacientes");
            using var form = new EntityDialog(current is null ? "Registrar pago" : "Editar pago");
            var patient = form.AddChoice("Paciente", patients.Select(p => new ChoiceItem(p.PacienteId, $"{p.Nombre} {p.Apellido} · #{p.PacienteId}")).ToList(), current?.PacienteId ?? current?.Cita?.PacienteId ?? current?.Cirugia?.PacienteId ?? pending?.PacienteId);
            var serviceNames = new[]
            {
                "Consulta",
                "Cirugía: Blefaroplastia",
                "Cirugía: Liposucción",
                "Cirugía: Rinoplastia",
                "Cirugía: Abdominoplastia",
                "Cirugía: Mamoplastia de aumento",
                "Cirugía: Mamoplastia de reducción",
                "Cirugía: Otoplastia",
                "Cirugía: Lifting facial",
                "Cirugía: Otro procedimiento"
            };
            var serviceChoices = serviceNames.Select((name, index) => new ChoiceItem(index, name)).ToList();
            var selectedService = serviceChoices.FirstOrDefault(option => string.Equals(option.Label, current?.Concepto, StringComparison.OrdinalIgnoreCase));
            if (selectedService is null && !string.IsNullOrWhiteSpace(current?.Concepto))
            {
                selectedService = new ChoiceItem(serviceChoices.Count, current.Concepto);
                serviceChoices.Add(selectedService);
            }
            var defaultServiceId = selectedService?.Id ?? (current?.CirugiaId is not null ? serviceChoices.FindIndex(option => option.Label == "Cirugía: Otro procedimiento") : 0);
            var service = form.AddChoice("Tipo de servicio", serviceChoices, defaultServiceId);
            var amount = form.AddText("Monto recibido", current?.Monto.ToString("0.00") ?? "0.00"); var date = form.AddDateTime("Fecha", current?.Fecha ?? DateTime.Now);
            var method = form.AddChoiceText("Método de pago", ["Efectivo", "Tarjeta", "Transferencia", "Cheque", "Seguro", "Otro"], current?.MetodoPago ?? "Efectivo");
            var state = form.AddChoiceText("Estado", ["Pagado", "Pendiente"], current?.Estado ?? "Pagado");
            var details = form.AddText("Detalle adicional (opcional)", pending is null ? null : "Abono de consulta", true);
            if (form.ShowDialog(this) != DialogResult.OK) return;
            if (patient.SelectedItem is not ChoiceItem p || !decimal.TryParse(amount.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var value) || value <= 0) throw new InvalidOperationException("Selecciona un paciente e indica un monto mayor que cero.");
            var selectedServiceLabel = (service.SelectedItem as ChoiceItem)?.Label ?? throw new InvalidOperationException("Selecciona Consulta o un tipo de cirugía.");
            var detail = EmptyNull(details.Text);
            if (selectedServiceLabel == "Cirugía: Otro procedimiento" && detail is null)
                throw new InvalidOperationException("Describe el procedimiento seleccionado como otro.");
            var concept = detail is null ? selectedServiceLabel : $"{selectedServiceLabel} · {detail}";
            var citaId = current?.CitaId ?? pending?.CitaId;
            var surgeryId = current?.CirugiaId ?? pending?.CirugiaId;
            await Run(async () => { await _api.SendAsync(HttpMethod.Post, "api/Pagos", new { PagoId = current?.PagoId ?? 0, PacienteId = p.Id, CitaId = citaId, CirugiaId = surgeryId, Fecha = date.Value, Monto = value, MontoSeguro = 0m, Estado = state.Text, MetodoPago = method.Text, Concepto = concept }); MessageBox.Show(this, "Pago guardado.", "Tanger Clinic"); await LoadSection("Pagos"); });
        }
        catch (Exception ex) { MessageBox.Show(this, UserMessage(ex), "No se pudo abrir el formulario", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private async Task DeleteRecord(string path, string prompt, Func<Task>? refresh = null)
    {
        if (MessageBox.Show(this, prompt, "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        await Run(async () => { await _api.DeleteAsync(path); MessageBox.Show(this, "Registro eliminado.", "Tanger Clinic"); if (refresh is null) await LoadSection(_section); else await refresh(); });
    }

    private async Task ChangeAppointmentStatus(int id)
    {
        var states = new[] { "Pendiente", "Confirmada", "Llegó", "Atendida", "Completada", "Cancelada" };
        using var form = new EntityDialog($"Estado de la cita #{id}"); var state = form.AddChoiceText("Nuevo estado", states, states[0]);
        if (form.ShowDialog(this) != DialogResult.OK) return;
        await Run(async () => { await _api.SendAsync(HttpMethod.Patch, $"api/Citas/{id}/estado", state.Text); await LoadSection("Citas"); });
    }
}
