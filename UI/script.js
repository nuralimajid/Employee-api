
const apiUrl = "http://localhost:5080/api/Employee";

function loadEmployees() {
  $.get(apiUrl, data => renderTable(data));
}

function searchEmployees() {
  const name = $("#searchName").val();
  const dept = $("#searchDept").val();
  if (!name && !dept) {
    alert("Isi setidaknya salah satu kolom pencarian.");
    return;
  }
  $.get(`${apiUrl}/search?name=${name}&department=${dept}`)
    .done(renderTable)
    .fail(xhr => {
      if (xhr.status === 404) {
        alert("Data tidak ditemukan.");
        $("#employeeTableBody").empty();
      } else {
        alert("Terjadi kesalahan saat pencarian.");
      }
    });
}

function deleteEmployee(id) {
  if (!confirm("Yakin mau hapus data ini?")) return;
  $.ajax({
    url: `${apiUrl}/${id}`,
    type: "DELETE",
    success: () => loadEmployees(),
    error: () => alert("Gagal menghapus data.")
  });
}

function renderTable(data) {
  const tbody = $("#employeeTableBody");
  tbody.empty();
  data.forEach(emp => {
    tbody.append(`
      <tr>
        <td>${emp.employeeId}</td>
        <td>${emp.name}</td>
        <td>${emp.department}</td>
        <td>${emp.email}</td>
        <td>
          <a class="btn btn-info btn-sm me-2" href="detail.html?id=${emp.employeeId}">Detail</a>
          <a class="btn btn-warning btn-sm me-2" href="form.html?id=${emp.employeeId}">Edit</a>
          <button class="btn btn-danger btn-sm" onclick="deleteEmployee('${emp.employeeId}')">Delete</button>
        </td>
      </tr>
    `);
  });
}

$(document).ready(loadEmployees);
