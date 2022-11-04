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
