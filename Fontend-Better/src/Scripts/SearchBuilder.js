function GenerateDefaultSearchBody() {
    return {
        nameQuery: null,
        descriptionQuery: null,
        typeQueries: null,
        usePartialTypeMatching: false,
        colorQuery: null,
        colorConstraint: 0,
        uniqueStrategy: 0,
        sortMode: 0,
        sortAscending: true,
        page: 0
    };
}

function BuildTypeQuery(type, include) {
    return { type, include };
}

function IsEmptySearchBody(searchBody) {
    if (!(searchBody.nameQuery === undefined || searchBody.nameQuery === null || searchBody.nameQuery === ""))
        return false;
    if (!(searchBody.descriptionQuery === undefined || searchBody.descriptionQuery === null || searchBody.descriptionQuery === ""))
        return false;
    if (!(searchBody.colorQuery === undefined || searchBody.colorQuery === null || searchBody.colorQuery === ""))
        return false;
    if (!(searchBody.typeQueries === undefined || searchBody.typeQueries === null || searchBody.typeQueries === []))
        return false;
    return true;
}

export { GenerateDefaultSearchBody, BuildTypeQuery, IsEmptySearchBody };