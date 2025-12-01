function buscarEnTabla(idInput, idTabla) {
    // Declarar variables 
    var input, filter, table, tr, td, i, j, visible;
    input = document.getElementById(idInput);
    filter = input.value.toUpperCase();
    table = document.getElementById(idTabla);
    tr = table.getElementsByTagName("tr");

    // Recorre todas las filas de la tabla y oculte aquellas que no coincidan con la consulta de búsqueda
    for (i = 1; i < tr.length; i++) {
        visible = false;
        /* Obtenemos todas las celdas de la fila, no sólo la primera */
        td = tr[i].getElementsByTagName("td");
        for (j = 0; j < td.length; j++) {
            if (td[j] && td[j].innerHTML.toUpperCase().indexOf(filter) > -1) {
                visible = true;
            }
        }
        if (visible === true) {
            tr[i].style.display = "";
        } else {
            tr[i].style.display = "none";
        }
    }
}