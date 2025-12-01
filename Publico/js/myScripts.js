/*Redimensión de carrusel servicios*/
$("#carruselServicios").on("slide.bs.carousel", function (e) {
    var $e = $(e.relatedTarget);
    var idx = $e.index();
    var itemsPerSlide = 3;
    var totalItems = $("#carruselServicios .carousel-item").length;
    if (idx >= totalItems - (itemsPerSlide - 1)) {
        var it = itemsPerSlide -
            (totalItems - idx);
        for (var i = 0; i < it; i++) {
            // append slides to end 
            if (e.direction == "left") {
                $("#carruselServicios .carousel-item").eq(i).appendTo("#carruselServicios .carousel-inner");
            } else {
                $("#carruselServicios .carousel-item").eq(0).appendTo("#carruselServicios .carousel-inner");
            }
        }
    }
});

/*DataTables*/
$('#tablaFacturas').dataTable({
    "lengthMenu": [10, 25, 50, 100],
    "pageLength": 10,
    language: {
        "processing": "Procesando...",
        "lengthMenu": "Mostrar _MENU_ registros",
        "zeroRecords": "No se encontraron resultados",
        "emptyTable": "Ningun dato disponible en esta tabla",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
        "infoFiltered": "filtrados de un total de _MAX_ registros",
        "search": "Buscar:",
        "infoThousands": ",",
        "loadingRecords": "Cargando...",
        "paginate": {
            "first": "Primero",
            "last": "Último",
            "next": "Siguiente",
            "previous": "Anterior"
        },
        "aria": {
            "sortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    }
});

$('#tablaClientes').dataTable({
    "lengthMenu": [10, 25, 50, 100],
    "pageLength": 10,
    language: {
        "processing": "Procesando...",
        "lengthMenu": "Mostrar _MENU_ registros",
        "zeroRecords": "No se encontraron resultados",
        "emptyTable": "Ningún dato disponible en esta tabla",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
        "infoFiltered": "filtrados de un total de _MAX_ registros",
        "search": "Buscar:",
        "infoThousands": ",",
        "loadingRecords": "Cargando...",
        "paginate": {
            "first": "Primero",
            "last": "Último",
            "next": "Siguiente",
            "previous": "Anterior"
        },
        "aria": {
            "sortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    }
});

$('#tablaProductos').dataTable({
    "lengthMenu": [10, 25, 50, 100],
    "pageLength": 10,
    language: {
        "processing": "Procesando...",
        "lengthMenu": "Mostrar _MENU_ registros",
        "zeroRecords": "No se encontraron resultados",
        "emptyTable": "Ningún dato disponible en esta tabla",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
        "infoFiltered": "filtrados de un total de _MAX_ registros",
        "search": "Buscar:",
        "infoThousands": ",",
        "loadingRecords": "Cargando...",
        "paginate": {
            "first": "Primero",
            "last": "Último",
            "next": "Siguiente",
            "previous": "Anterior"
        },
        "aria": {
            "sortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    }
});

$('#tablaUsuarios').dataTable({
    "lengthMenu": [10, 25, 50, 100],
    "pageLength": 10,
    language: {
        "processing": "Procesando...",
        "lengthMenu": "Mostrar _MENU_ registros",
        "zeroRecords": "No se encontraron resultados",
        "emptyTable": "Ningun dato disponible en esta tabla",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
        "infoFiltered": "filtrados de un total de _MAX_ registros",
        "search": "Buscar:",
        "infoThousands": ",",
        "loadingRecords": "Cargando...",
        "paginate": {
            "first": "Primero",
            "last": "Último",
            "next": "Siguiente",
            "previous": "Anterior"
        },
        "aria": {
            "sortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    }
});

$('#tabla_cobrosEnviados').dataTable({
    "lengthMenu": [10, 25, 50, 100],
    "pageLength": 10,
    language: {
        "processing": "Procesando...",
        "lengthMenu": "Mostrar _MENU_ registros",
        "zeroRecords": "No se encontraron resultados",
        "emptyTable": "Ningun dato disponible en esta tabla",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
        "infoFiltered": "filtrados de un total de _MAX_ registros",
        "search": "Buscar:",
        "infoThousands": ",",
        "loadingRecords": "Cargando...",
        "paginate": {
            "first": "Primero",
            "last": "Último",
            "next": "Siguiente",
            "previous": "Anterior"
        },
        "aria": {
            "sortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    }
});

$('#tablaCuentas').dataTable({
    "lengthMenu": [10, 25, 50, 100],
    "pageLength": 10,
    language: {
        "processing": "Procesando...",
        "lengthMenu": "Mostrar _MENU_ registros",
        "zeroRecords": "No se encontraron resultados",
        "emptyTable": "Ningun dato disponible en esta tabla",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
        "infoFiltered": "filtrados de un total de _MAX_ registros",
        "search": "Buscar:",
        "infoThousands": ",",
        "loadingRecords": "Cargando...",
        "paginate": {
            "first": "Primero",
            "last": "Último",
            "next": "Siguiente",
            "previous": "Anterior"
        },
        "aria": {
            "sortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    }
});

$('#tablaDB_Clientes').dataTable({
    "lengthMenu": [10, 25, 50, 100],
    "pageLength": 10,
    language: {
        "processing": "Procesando...",
        "lengthMenu": "Mostrar _MENU_ registros",
        "zeroRecords": "No se encontraron resultados",
        "emptyTable": "Ningun dato disponible en esta tabla",
        "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
        "infoFiltered": "filtrados de un total de _MAX_ registros",
        "search": "Buscar:",
        "infoThousands": ",",
        "loadingRecords": "Cargando...",
        "paginate": {
            "first": "Primero",
            "last": "Último",
            "next": "Siguiente",
            "previous": "Anterior"
        },
        "aria": {
            "sortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    }
});



