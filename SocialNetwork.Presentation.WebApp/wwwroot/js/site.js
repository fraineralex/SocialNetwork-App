let restorePassBtn = document.querySelector("#restore-pass");

restorePassBtn.addEventListener("click", async () => {
  const { value: username } = await Swal.fire({
    title: "Input your username to restore your password",
      html: `<form method="post" action="User/RestorePassword" id="frm-restore-password"> 
          <input id="content" type="text" class="form-control border-secondary border border-2" placeholder="Enter your username" name="Username" required>
          </form>`,
    showCancelButton: true,
    focusConfirm: false,
    preConfirm: () => {
      return [document.getElementById("content").value];
    },
  });

  if (username) {
    if (username.filter(Boolean).length < 1) {
      Swal.fire("Error!", "The field username can't be empty", "error");
    } else {
      let form = document.querySelector("#frm-restore-password");
      form.submit();
    }
  }
});

function DeleteConfirm(friendId) {
    Swal.fire({
        title: `Are you sure you want to delete this friend?`,
        text: "Once it has been deleted it cannot be recovered.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Delete",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire("Deleted!", `Friend successfully deleted!`, "success");

            setTimeout(() => {
                let form = document.createElement("form");
                form.action = `AdminFriends/DeleteFriend?receptorId=${friendId}`;
                form.method = "POST";
                document.body.append(form);
                form.submit();
            }, 2000);
        }
    });
}