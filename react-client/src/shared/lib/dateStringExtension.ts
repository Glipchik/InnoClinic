Object.defineProperty(Date.prototype, "toISODateString", {
  value: function toISODateString() {
    return this.toISOString().split("T")[0]; 
  },
  writable: true,
  configurable: true,
});

export {};

declare global {
  interface Date {
    toISODateString() : string;
  }
}

Date.prototype.toISODateString = function () {
  return this.toISOString().split("T")[0]; 
}