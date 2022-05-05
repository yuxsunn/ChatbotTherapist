var FirebaseInterface = {

  writeFile: function (path, file, data) {
    console.log("Write file to firebase");
    console.log(Pointer_stringify(path).concat('/', Pointer_stringify(file)));
    var storage = firebase.storage();
    var storageRef = storage.ref();

    var spaceRef = storageRef.child(Pointer_stringify(path).concat('/', Pointer_stringify(file)));

    var myFile = new File([Pointer_stringify(data)], Pointer_stringify(file), {type: 'text/plain'});

    try {
      var uploadTask = spaceRef.put(myFile);
	  console.log("put myfile");
	  uploadTask.on('state_changed', function progress(snapshot) {
     console.log(snapshot.totalBytesTransferred); // progress of upload
	 });
    } catch (error) {
      console.error(error);
    }
  }
};

mergeInto(LibraryManager.library, FirebaseInterface);


