function getPluralizedStringFromCount(
  count: number,
  singular: string,
  plural: string
): string {
  if (count < 0) {
    return "";
  }

  const isPlural = count == 0 || count > 1;
  return isPlural ? plural : singular;
}

export const PluralizedStringUtils = {
  getPluralizedStringFromCount
};
