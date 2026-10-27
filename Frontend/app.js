const API_BASE = "http://localhost:5269/api";

async function obtenerJson(url) {
  const response = await fetch(url);
  if (!response.ok) {
    throw new Error(`Error HTTP: ${response.status}`);
  }
  return response.json();
}

function mostrarError(texto) {
  const div = document.getElementById("mensaje-error");
  div.textContent = texto;
  div.hidden = false;
}

function renderizarTalleres(talleres) {
  const tbody = document.getElementById("tabla-talleres");
  if (talleres.length === 0) {
    tbody.innerHTML = `<tr><td colspan="4">No hay talleres cargados.</td></tr>`;
    return;
  }
  tbody.innerHTML = talleres
    .map(t => `
      <tr>
        <td>${t.nombre}</td>
        <td>${t.dia}</td>
        <td>${t.horaInicio}:00 a ${t.horaFin}:00</td>
        <td class="${t.cuposDisponibles > 0 ? "cupo-libre" : "cupo-lleno"}">
          ${t.cuposDisponibles} de ${t.cupo}
        </td>
      </tr>
    `)
    .join("");
}

async function cargarTalleres() {
  const estado = document.getElementById("estado-carga");
  try {
    const talleres = await obtenerJson(`${API_BASE}/talleres`);
    estado.textContent = "";
    renderizarTalleres(talleres);
  } catch (error) {
    estado.textContent = "";
    mostrarError("No se pudieron cargar los talleres. Verificá que la API esté corriendo.");
    console.error(error);
  }
}

document.addEventListener("DOMContentLoaded", cargarTalleres);
