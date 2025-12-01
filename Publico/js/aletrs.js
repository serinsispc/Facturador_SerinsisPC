function alerta(titulo, mensage, tipo, url) {
    swal(titulo, mensage, tipo).then(function () { window.location = url });
}

function alerta2(titulo, mensage, tipo) {
    swal(titulo, mensage, tipo);
}

function Modal() {
    /*alert('hola');*/
  $('#exampleModal').modal('show');
}

