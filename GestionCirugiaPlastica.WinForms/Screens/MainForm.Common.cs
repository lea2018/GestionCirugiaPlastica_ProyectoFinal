using System.Drawing;
using System.Globalization;
using System.Text.Json;

namespace GestionCirugiaPlastica.WinForms;

internal partial class MainForm
{
    private static DateTime ReadDate(JsonElement value, string name) => FindProperty(value, name).ValueKind == JsonValueKind.String && DateTime.TryParse(ReadText(value, name), out var date) ? date : DateTime.MinValue;

    private static string FormatRecord(JsonElement root)
    {
        var output = new System.Text.StringBuilder();
        var p = FindProperty(root, "paciente");
        if (p.ValueKind == JsonValueKind.Object) output.AppendLine($"PACIENTE\r\n{ReadText(p, "nombre")} {ReadText(p, "apellido")}  |  ID #{ReadInt(p, "pacienteId")}\r\nCédula: {ReadText(p, "cedula")}  |  Teléfono: {ReadText(p, "telefono")}  |  Correo: {ReadText(p, "email")}\r\n");
        var insurance = FindProperty(root, "seguro"); var policies = FindProperty(insurance, "polizas");
        if (insurance.ValueKind == JsonValueKind.Object) output.AppendLine($"SEGURO\r\nTiene seguro: {FindProperty(insurance, "tieneSeguro").ToString()}\r\nPólizas: {policies}");
        AppendArray(root, "citas", "CITAS", output); AppendArray(root, "operaciones", "CIRUGÍAS", output); AppendArray(root, "consultas", "CONSULTAS Y DIAGNÓSTICOS", output); AppendArray(root, "historial", "HISTORIAL CLÍNICO", output); AppendArray(root, "fotografias", "FOTOGRAFÍAS", output); AppendArray(root, "preoperatorios", "EVALUACIONES PREOPERATORIAS", output); AppendArray(root, "controles", "CONTROLES POSTOPERATORIOS", output); AppendArray(root, "recetas", "RECETAS", output); AppendArray(root, "documentos", "DOCUMENTOS", output);
        return output.ToString();
    }
    private static void AppendArray(JsonElement root, string key, string label, System.Text.StringBuilder output)
    {
        output.AppendLine(label);
        var items = FindProperty(root, key);
        if (items.ValueKind == JsonValueKind.Array && items.GetArrayLength() > 0)
            foreach (var item in items.EnumerateArray()) output.AppendLine("• " + item.ToString());
        else if (items.ValueKind == JsonValueKind.Object) output.AppendLine(items.ToString());
        else output.AppendLine("Sin registros.");
        output.AppendLine();
    }

    private static JsonElement FindProperty(JsonElement value, string name)
    {
        if (value.ValueKind == JsonValueKind.Object)
            foreach (var property in value.EnumerateObject()) if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase)) return property.Value;
        return default;
    }
    private static int ReadInt(JsonElement value, string name) { var item = FindProperty(value, name); return item.ValueKind == JsonValueKind.Number && item.TryGetInt32(out var number) ? number : 0; }
    private static string? ReadText(JsonElement value, string name) { var item = FindProperty(value, name); return item.ValueKind == JsonValueKind.String ? item.GetString() : null; }

    private async Task Run(Func<Task> operation)
    {
        try { await operation(); }
        catch (Exception ex) { MessageBox.Show(this, UserMessage(ex), "Tanger Clinic", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
    private string UserMessage(Exception ex) => ex is HttpRequestException ? $"No se pudo conectar con la API en {_api.BaseAddress}. Verifica que esté iniciada y que su URL base sea correcta." : ex.Message;
    private static string? EmptyNull(string text) => string.IsNullOrWhiteSpace(text) ? null : text.Trim();
    private static string FormatDateTime12(DateTime value) => value.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
    private static Button Button(string text, Color back, Color fore) => new() { Text = text, BackColor = back, ForeColor = fore, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand, UseVisualStyleBackColor = false };
}
