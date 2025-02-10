export function addDays(date: Date, days: number): Date {
  const newDate = new Date(date)
  newDate.setDate(date.getDate() + days)
  return newDate
}

export const MIN_APPOINTMENT_DATE = addDays(new Date(), 2)