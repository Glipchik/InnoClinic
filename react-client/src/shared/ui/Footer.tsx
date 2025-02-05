import React from "react";

function Footer() {
  return (
    <footer className="bg-gray-700 text-white py-4 text-center">
      <p className="text-sm">&copy; {new Date().getFullYear()} InnoClinic. Все права защищены.</p>
    </footer>
  );
}

export default Footer;
  