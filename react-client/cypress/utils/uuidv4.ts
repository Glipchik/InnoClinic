const uuidv4 = () => {
  return "10000000-1000-4000-8000".replace(/[018]/g, c =>
    (+c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> +c / 4).toString(16)
  );
}
 
export default uuidv4;