// Validacion de extensiones permitidas
function validarExtension(datos) {
    var ruta = datos.value;
    var extension = ruta.substring(ruta.lastIndexOf('.') + 1).toLowerCase();
    var extensionValida = extensionesValidas.indexOf(extension);
    if (extensionValida < 0) {
        $('#texto').text('La extensión no es válida Su fichero tiene de extensión: .' + extension);
        return false;
    } else {
        return true;
    }
}

// Validacion de peso del fichero en kbs
function validarPeso(datos) {
    if (datos.files && datos.files[0]) {
        var pesoFichero = datos.files[0].size / 1024;
        if (pesoFichero > pesoPermitido) {
            $('#texto').text('El peso maximo permitido del fichero es: ' + pesoPermitido + ' KBs Su fichero tiene: ' + pesoFichero + ' KBs');
            return false;
        } else {
            return true;
        }
    }
}

// Vista preliminar de la imagen.
function verImagen(datos) {
    if (datos.files && datos.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#img').attr('src', e.target.result);
        };
        reader.readAsDataURL(datos.files[0]);
    }
}